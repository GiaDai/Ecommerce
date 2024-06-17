using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Domain.Globals;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProdByIdEvent : INotification
    {
        public int ProductId { get; set; }
    }
    public class DeleteProdByIdEventHandler : INotificationHandler<DeleteProdByIdEvent>
    {
        private readonly IDistributedCache _distributedCache;
        public DeleteProdByIdEventHandler(
            IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task Handle(DeleteProdByIdEvent notification, CancellationToken cancellationToken)
        {
            if (!RedisConnectionMonitor.IsRedisConnected) return;
            var cacheKey = $"product_{notification.ProductId}";
            _distributedCache.Remove(cacheKey);
            await Task.CompletedTask;
        }
    }
}
