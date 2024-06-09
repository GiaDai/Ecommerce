using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Ecommerce.Infrastructure.Shared.Environments;
using Ecommerce.Infrastructure.Shared.Services;
using MassTransit;
using RabbitMQ.Client;

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

        public static void AddRabbitMqExtension(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _rabbitmqSetting = scope.ServiceProvider.GetRequiredService<IRabbitMqSettingProdiver>();
                string rabbitHost = _rabbitmqSetting.GetHostName();
                string rabbitvHost = _rabbitmqSetting.GetVHost();
                string rabbitUser = _rabbitmqSetting.GetUserName();
                string rabbitPass = _rabbitmqSetting.GetPassword();

                services.AddMassTransit(x =>
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                    {
                        config.Host(rabbitHost, rabbitvHost, h =>
                        {
                            h.Username(rabbitUser);
                            h.Password(rabbitPass);
                        });

                    }));
                });
                services.AddMassTransitHostedService();
            }
        }

        public static void AddRabbitMqFactoryExtension(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _rabbitmqSetting = scope.ServiceProvider.GetRequiredService<IRabbitMqSettingProdiver>();
                services.AddSingleton<IConnectionFactory>(provider =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _rabbitmqSetting.GetHostName(),
                        Port = int.Parse(_rabbitmqSetting.GetPort()),
                        VirtualHost = _rabbitmqSetting.GetVHost(),
                        UserName = _rabbitmqSetting.GetUserName(),
                        Password = _rabbitmqSetting.GetPassword()
                    };
                    Console.WriteLine($"RabbitMq Connection Factory Created: {factory.HostName}");

                    return factory;
                });
            }
            services.AddTransient<IRabbitMqService, RabbitMqService>();
        }

        public static void AddEnvironmentVariablesExtension(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseSettingsProvider, DatabaseSettingsProvider>();
            services.AddTransient<IRedisSettingsProvider, RedisSettingsProvider>();
            services.AddTransient<IElasticSettingsProvider, ElasticSettingsProvider>();
            services.AddTransient<ICloudinarySettingsProvider, CloudinarySettingsProvider>();
            services.AddTransient<IRabbitMqSettingProdiver, RabbitMqSettingProdiver>();
        }
    }
}
