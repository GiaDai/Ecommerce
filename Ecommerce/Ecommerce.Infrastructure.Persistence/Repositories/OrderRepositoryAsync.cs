using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Ecommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepositoryAsync : GenericRepositoryAsync<Order>, IOrderRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Order> _orders;
        private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(10, 10); // Khóa đồng bộ hóa
        // Định nghĩa một hàng đợi để lưu trữ các yêu cầu
        private readonly Queue<int> _requestQueue = new Queue<int>();
        public OrderRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _orders = dbContext.Set<Order>();
        }

        public async Task<int> PlaceOrderAsync(int productId)
        {
            _requestQueue.Enqueue(productId); // Thêm yêu cầu vào hàng đợi

            await ProcessQueueOtherAsync(); // Xử lý hàng đợi

            return 1; // Hoặc giá trị tùy ý để biểu thị việc đặt hàng đã hoàn thành
        }

        private async Task ProcessQueueAsync()
        {
            while (_requestQueue.Count > 0)
            {
                // Kiểm tra xem Semaphore có thể nhận thêm yêu cầu không
                if (_processingSemaphore.CurrentCount == 0)
                {
                    // Đợi một khoảng thời gian ngắn trước khi kiểm tra lại
                    await Task.Delay(100);
                    continue;
                }

                await _processingSemaphore.WaitAsync(); // Chờ để nhận khóa đồng bộ hóa

                try
                {
                    var productId = _requestQueue.Dequeue(); // Lấy yêu cầu từ hàng đợi
                    var strategy = _dbContext.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        // Số lần thử lại khi gặp deadlock
                        bool success = false;
                        int retryCount = 3;
                        for (int attempt = 0; attempt < retryCount; attempt++)
                        {
                            try
                            {
                                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                                {
                                    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

                                    // Xử lý logic đặt hàng ở đây
                                    if (product != null && product.Stock > 0)
                                    {
                                        // Giảm số lượng hàng tồn của sản phẩm
                                        Console.WriteLine("##################################");
                                        Console.WriteLine("Before: " + product.Stock);
                                        product.Stock -= 1;
                                        Console.WriteLine("After: " + product.Stock);
                                        Console.WriteLine("##################################");
                                        _dbContext.Products.Update(product);

                                        // Tạo đơn hàng
                                        var order = new Order
                                        {
                                            ProductId = productId
                                        };
                                        await _orders.AddAsync(order);
                                        await _dbContext.SaveChangesAsync();

                                        // Commit transaction
                                        await transaction.CommitAsync();
                                        success = true;
                                    }
                                }
                                // Nếu thành công, thoát khỏi vòng lặp
                                if (success)
                                {
                                    break;
                                }
                            }
                            catch (MySqlException ex) when (ex.Number == 1213)
                            {
                                // Xử lý deadlock: thử lại giao dịch
                                Console.WriteLine("Deadlock detected, retrying transaction...");
                                if (attempt == retryCount - 1)
                                    throw;
                                await Task.Delay(100); // Đợi một khoảng thời gian trước khi thử lại
                            }
                        }
                    });
                }
                finally
                {
                    _processingSemaphore.Release(); // Giải phóng khóa đồng bộ hóa
                }
            }
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
                    var productId = _requestQueue.Dequeue();
                    var strategy = _dbContext.Database.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        await ExecuteTransactionAsync(productId);
                    });
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

        private async Task ExecuteTransactionAsync(int productId)
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
    }
}
