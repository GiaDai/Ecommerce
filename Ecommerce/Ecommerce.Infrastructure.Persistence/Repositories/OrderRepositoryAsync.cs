using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Ecommerce.Infrastructure.Shared;
using Ecommerce.Infrastructure.Shared.Environments;
using Ecommerce.Infrastructure.Shared.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Newtonsoft.Json;
using Npgsql;
using RabbitMQ.Client;

namespace Ecommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepositoryAsync : GenericRepositoryAsync<Order>, IOrderRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Order> _orders;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IRabbitMqSettingProdiver _rabbitMqSettingProdiver;
        private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(10, 10); // Khóa đồng bộ hóa
        // Định nghĩa một hàng đợi để lưu trữ các yêu cầu
        // private readonly Queue<int> _requestQueue = new Queue<int>();
        // Định nghĩa một hàng đợi để lưu trữ các yêu cầu
        private readonly ConcurrentQueue<(int ProductId, TaskCompletionSource<int> CompletionSource)> _requestQueue = new ConcurrentQueue<(int ProductId, TaskCompletionSource<int> CompletionSource)>();
        public OrderRepositoryAsync(
            ApplicationDbContext dbContext,
            IRabbitMqService rabbitMqService,
            IRabbitMqSettingProdiver rabbitMqSettingProdiver
            ) : base(dbContext)
        {
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
            _rabbitMqSettingProdiver = rabbitMqSettingProdiver;
            _orders = dbContext.Set<Order>();
        }

        public int PlaceOrderForwardAsync(int productId)
        {
            var orderRequest = new { ProductId = productId };
            var message = JsonConvert.SerializeObject(orderRequest);
            _rabbitMqService.Publish("order_queue", message);

            return 1;
        }


        public async Task<int> PlaceOrderAsync(int productId)
        {
            var completionSource = new TaskCompletionSource<int>();
            _requestQueue.Enqueue((productId, completionSource)); // Thêm yêu cầu vào hàng đợi

            await ProcessQueueOtherAsync(); // Xử lý hàng đợi

            return await completionSource.Task; // Hoặc giá trị tùy ý để biểu thị việc đặt hàng đã hoàn thành
        }

        private async Task ProcessQueueOtherAsync()
        {
            while (_requestQueue.Count > 0)
            {
                if (_processingSemaphore.CurrentCount == 0)
                {
                    await Task.Delay(100);
                    continue;
                }

                await _processingSemaphore.WaitAsync();

                try
                {
                    if (_requestQueue.TryDequeue(out var request))
                    {
                        var (productId, completionSource) = request;
                        var strategy = _dbContext.Database.CreateExecutionStrategy();

                        await strategy.ExecuteAsync(async () =>
                        {
                            await ExecuteTransactionAsync(productId, completionSource);
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    _processingSemaphore.Release();
                }
            }
        }

        private async Task ExecuteTransactionAsync(int productId, TaskCompletionSource<int> completionSource)
        {
            bool success = false;
            int retryCount = 3;

            for (int attempt = 0; attempt < retryCount; attempt++)
            {
                try
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var product = await _dbContext.Products
                                .FromSqlRaw("SELECT * FROM Products WHERE Id = {0} FOR UPDATE", productId)
                                .FirstOrDefaultAsync();

                            if (product != null && product.Stock > 0)
                            {
                                Console.WriteLine("##################################");
                                Console.WriteLine("Before: " + product.Stock);
                                product.Stock -= 1;
                                Console.WriteLine("After: " + product.Stock);
                                Console.WriteLine("##################################");
                                _dbContext.Products.Update(product);

                                var order = new Order
                                {
                                    ProductId = productId
                                };
                                await _orders.AddAsync(order);
                                await _dbContext.SaveChangesAsync();
                                await transaction.CommitAsync();
                                success = true;
                                completionSource.SetResult(order.Id);
                            }
                            else
                            {
                                completionSource.SetResult(0);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            completionSource.SetResult(0);
                            await transaction.RollbackAsync();
                        }
                    }

                    if (success)
                    {
                        break;
                    }
                }
                catch (MySqlException ex) when (ex.Number == 1213)
                {
                    Console.WriteLine("Deadlock detected, retrying transaction...");
                    if (attempt == retryCount - 1)
                    {
                        throw;
                    }
                    await Task.Delay(100);
                }
            }
        }

        private async Task ExecuteTransactionPostgreAsync(int productId)
        {
            bool success = false;
            int retryCount = 3;

            for (int attempt = 0; attempt < retryCount; attempt++)
            {
                try
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        var product = await _dbContext.Products
                            .FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = {0} FOR UPDATE", productId)
                            .FirstOrDefaultAsync();

                        if (product != null && product.Stock > 0)
                        {
                            Console.WriteLine("##################################");
                            Console.WriteLine("Before: " + product.Stock);
                            product.Stock -= 1;
                            Console.WriteLine("After: " + product.Stock);
                            Console.WriteLine("##################################");
                            _dbContext.Products.Update(product);

                            var order = new Order
                            {
                                ProductId = productId
                            };
                            await _orders.AddAsync(order);
                            await _dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                            success = true;
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }
                    }

                    if (success)
                    {
                        break;
                    }
                }
                catch (PostgresException ex) when (ex.SqlState == "40P01")
                {
                    Console.WriteLine("Deadlock detected, retrying transaction...");
                    if (attempt == retryCount - 1)
                    {
                        throw;
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}
