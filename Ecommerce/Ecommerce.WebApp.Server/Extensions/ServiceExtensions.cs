using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Ecommerce.Infrastructure.Shared.Environments;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis;
using CacheManager.Core;
using CacheManager.Core.Internal;
using Ecommerce.WebApp.Server.Services;
using Ecommerce.Domain.Globals;

namespace Ecommerce.WebApp.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"Ecommerce.WebApp.Server.xml"));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Clean Architecture - Ecommerce.WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "codewithmukesh",
                        Email = "hello@codewithmukesh.com",
                        Url = new Uri("https://codewithmukesh.com/contact"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }

        public static void AddRedisCacheExtension(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _redisSetting = scope.ServiceProvider.GetRequiredService<IRedisSettingsProvider>();
                var redisConfig = _redisSetting.GetRedisConfiguration();

                // Kiểm tra kết nối đến Redis
                try
                {
                    var redis = ConnectionMultiplexer.Connect(redisConfig.ConfigurationOptions);
                    if (redis.IsConnected)
                    {
                        Console.WriteLine($"Connected to Redis Server, {redisConfig.ConfigurationOptions}");

                        // Đăng ký Redis cache
                        services.AddStackExchangeRedisCache(options =>
                        {
                            options.Configuration = redisConfig.ConfigurationOptions.ToString();
                        });

                        // Đăng ký IConnectionMultiplexer
                        services.AddSingleton<IConnectionMultiplexer>(sp =>
                            ConnectionMultiplexer.Connect(redisConfig.ConfigurationOptions));
                        redis.ConnectionFailed += (s, e) =>
                        {
                            RedisConnectionMonitor.IsRedisConnected = false;
                            Console.WriteLine("Redis connection failed.");
                            // Log lỗi kết nối Redis vào hệ thống log chính của bạn nếu cần thiết
                        };
                        redis.ConnectionRestored += (s, e) =>
                        {
                            RedisConnectionMonitor.IsRedisConnected = true;
                            Console.WriteLine("Redis connection restored.");
                            var subscriber = redis.GetSubscriber();
                            subscriber.Publish("product-refresh", "true");
                            // Log lỗi kết nối Redis vào hệ thống log chính của bạn nếu cần thiết
                        };
                        // Đăng ký RedisConnectionChecker
                        // services.AddSingleton<RedisConnectionChecker>();
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to Redis Server");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to Redis Server: {ex.Message}");
                    // Log lỗi kết nối Redis vào hệ thống log chính của bạn nếu cần thiết
                }
            }
        }


        public static void AddCacheManagerExtension(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _redisSetting = scope.ServiceProvider.GetRequiredService<IRedisSettingsProvider>();
                var redisConfig = _redisSetting.GetRedisConfiguration();

                var cacheManagerConfiguration = CacheManager.Core.ConfigurationBuilder.BuildConfiguration(settings =>
                {
                    settings
                        .WithJsonSerializer() // Thiết lập JSON serializer cho CacheManager.
                        .WithUpdateMode(CacheUpdateMode.Up)
                        .WithMaxRetries(1000)
                        .WithRetryTimeout(100)
                        .WithRedisConfiguration("redis", config => //Cấu hình Redis sử dụng RedisConfigurationBuilder
                        {
                            config.WithAllowAdmin() //Cho phép các lệnh quản trị Redis.
                                .WithDatabase(0) //Sử dụng database số 0 trong Redis.
                                .WithEndpoint( //Thiết lập endpoint Redis
                                    redisConfig.Hosts[0].Host.ToString(),
                                    int.Parse(redisConfig.Hosts[0].Port.ToString())
                                )
                                .WithPassword(redisConfig.Password); // Nếu Redis của bạn có password, nếu không thì bỏ qua dòng này
                        })
                        .WithHandle(typeof(DictionaryCacheHandle<>)) //Sử dụng Dictionary cache handle.
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10)); //Thiết lập chế độ hết hạn là tuyệt đối và thời gian hết hạn là 10 phút.
                });

                services.AddSingleton(typeof(ICacheManagerConfiguration), cacheManagerConfiguration);
                services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            }
        }

        public static void AddEnvironmentVariablesExtension(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseSettingsProvider, DatabaseSettingsProvider>();
            services.AddTransient<IRedisSettingsProvider, RedisSettingsProvider>();
            services.AddTransient<IElasticSettingsProvider, ElasticSettingsProvider>();
            services.AddTransient<ICloudinarySettingsProvider, CloudinarySettingsProvider>();
        }
    }
}
