using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

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

            await ProcessQueueAsync(); // Xử lý hàng đợi

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
                    }
                }
                finally
                {
                    _processingSemaphore.Release(); // Giải phóng khóa đồng bộ hóa
                }
            }
        }

    }
}
