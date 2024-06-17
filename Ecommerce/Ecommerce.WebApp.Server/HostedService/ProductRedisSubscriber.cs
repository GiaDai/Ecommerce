
using System.Text.Json;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ecommerce.WebApp.Server.HostedService;

public class ProductRedisSubscriber : IHostedService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatabase _database;
    public ProductRedisSubscriber(
        IConnectionMultiplexer redis,
        IServiceProvider serviceProvider)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
        _serviceProvider = serviceProvider;
    }

    [Obsolete]
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var sub = _redis.GetSubscriber();
        sub.Subscribe("product-refresh", async (channel, message) =>
        {
            Console.WriteLine($"Received message: {message}");
            await RefreshProductCache();
        });
        return Task.CompletedTask;
    }

    private async Task RefreshProductCache()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _productRepository = scope.ServiceProvider.GetRequiredService<IProductRepositoryAsync>();
            var distributedCache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            var products = await _productRepository.GetAllAsync();
            Console.WriteLine($"RefreshProductCache: {products.Count()}");
            await SetProductsAsync(products);
        }
    }

    private async Task SetProductsAsync(IEnumerable<Product> products)
    {
        var cacheKey = "products";
        var hashEntries = products.Select(p => new HashEntry(p.Id.ToString(), JsonConvert.SerializeObject(p))).ToArray();

        var parameters = new List<object> { cacheKey };
        foreach (var entry in hashEntries)
        {
            parameters.Add(entry.Name);
            parameters.Add(entry.Value);
        }

        // Sử dụng HMSET để lưu trữ nhiều sản phẩm vào Redis Hash
        await _database.ExecuteAsync("HMSET", parameters.ToArray());
        // Đặt TTL cho key 'products'
        await _database.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(10));
    }

    private async Task<Product?> GetProductByIdAsync(int productId)
    {
        var cacheKey = "products";
        var field = productId.ToString();

        var cachedProduct = await _database.HashGetAsync(cacheKey, field);
        if (!cachedProduct.IsNull)
        {
            return JsonConvert.DeserializeObject<Product>(cachedProduct!);
        }

        // Handle cache miss: Fetch from database, update cache, and return product
        return null;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
