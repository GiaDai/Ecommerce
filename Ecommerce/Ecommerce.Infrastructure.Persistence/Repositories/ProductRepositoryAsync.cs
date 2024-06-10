using Microsoft.EntityFrameworkCore;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Ecommerce.Infrastructure.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System;
using System.Threading;
using System.Collections.Concurrent;

namespace Ecommerce.Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;
        private readonly ApplicationDbContext _dbContext;
        private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(10, 10); // Khóa đồng bộ hóa
        private readonly ConcurrentQueue<(int ProductId, TaskCompletionSource<int> CompletionSource)> _requestQueue = new ConcurrentQueue<(int ProductId, TaskCompletionSource<int> CompletionSource)>();

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _products = dbContext.Set<Product>();
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _products
                .AllAsync(p => p.Barcode != barcode);
        }

        public async Task<int> DeleteRangeAsync(List<int> ids)
        {
            var products = await _products.Where(p => ids.Contains(p.Id)).ToListAsync();
            _products.RemoveRange(products);
            return products.Count;
        }

        public async Task<PagedList<Product>> GetPagedProductsAsync(GetAllProductsParameter request)
        {
            var productQuery = _products.AsQueryable();
            if (request._filter != null && request._filter.Count > 0)
            {
                productQuery = MethodExtensions.ApplyFilters(productQuery, request._filter);
            }

            return await PagedList<Product>.ToPagedList(productQuery.OrderByDynamic(request._sort, request._order).AsNoTracking(), request._start, request._end);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await _products.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<int> PlaceOrderAsync(int productId, int quantity)
        {
            var completionSource = new TaskCompletionSource<int>();
            _requestQueue.Enqueue((productId, completionSource)); // Thêm yêu cầu vào hàng đợi

            await ProcessQueueAsync(quantity); // Xử lý hàng đợi

            return await completionSource.Task; // Hoặc giá trị tùy ý để biểu thị việc đặt hàng đã hoàn thành
        }

        private async Task ProcessQueueAsync(int quantity)
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
                            await ExecuteTransactionPostgreAsync(productId, quantity, completionSource);
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

        private async Task ExecuteTransactionPostgreAsync(int productId, int quantity, TaskCompletionSource<int> completionSource)
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
                                .FromSqlRaw("SELECT * FROM \"Products\" WHERE \"Id\" = {0} FOR UPDATE", productId)
                                .FirstOrDefaultAsync();

                            if (product != null && product.Stock > 0)
                            {
                                Console.WriteLine("##################################");
                                Console.WriteLine("Before: " + product.Stock);
                                product.Stock -= quantity;
                                Console.WriteLine("After: " + product.Stock);
                                Console.WriteLine("##################################");
                                _dbContext.Products.Update(product);
                                _dbContext.SaveChanges();
                                await transaction.CommitAsync();
                                success = true;
                                completionSource.SetResult(productId);
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
