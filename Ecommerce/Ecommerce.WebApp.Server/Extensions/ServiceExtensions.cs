using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Ecommerce.Infrastructure.Shared.Environments;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis;
using CacheManager.Core;

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
                Console.WriteLine($"Redis Host: {redisConfig.Hosts[0].Host}");
                Console.WriteLine($"Redis Port: {redisConfig.Hosts[0].Port}");
                Console.WriteLine($"Redis Password: {redisConfig.Password}");

                services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);

                // Add code to check connection to Redis server
                var redis = ConnectionMultiplexer.Connect(redisConfig.ConfigurationOptions);
                // Add code to check connection to Redis server
                var connection = ConnectionMultiplexer.Connect("localhost,password=redis");
                if (connection.IsConnected)
                {
                    Console.WriteLine("Connected to Redis Server");
                }
                else
                {
                    Console.WriteLine("Failed to connect to Redis Server");
                }
                // var server = redis.GetServer(redisConfig.ConfigurationOptions.EndPoints.First());
                // if (server.IsConnected)
                // {
                //     Console.WriteLine("Connected to Redis Server");
                // }
                // else
                // {
                //     Console.WriteLine("Failed to connect to Redis Server");
                // }
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
