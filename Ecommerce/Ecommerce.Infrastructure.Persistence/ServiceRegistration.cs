using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repositories;
using Ecommerce.Infrastructure.Persistence.Repository;
using Ecommerce.Infrastructure.Shared.Environments;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Common;

namespace Ecommerce.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddInMemoryDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));
        }
        public static void AddSqlServerPersistenceInfrastructure(this IServiceCollection services, string assembly)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string appConnStr = _dbSetting.GetSQLServerConnectionString();
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                appConnStr,
                b => b.MigrationsAssembly(assembly)));
            }
        }

        public static void AddMySqlPersistenceInfrastructure(this IServiceCollection services)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string appConnStr = _dbSetting.GetMySQLConnectionString();
                if (!string.IsNullOrWhiteSpace(appConnStr))
                {
                    var serverVersion = new MySqlServerVersion(new Version(5, 7, 35));
                    services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(
                        appConnStr, serverVersion,
                        b =>
                        {
                            b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                            b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                            b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        }));
                }
            }
        }

        public static void AddNpgSqlPersistenceInfrastructure(this IServiceCollection services, string assembly)
        {
            // Configure Redis Cache
            var redisConnectionString = "localhost,password=redis"; // Redis connection string
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            // Configure DbContext
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var _dbSetting = serviceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string appConnStr = _dbSetting.GetPostgresConnectionString();
                if (!string.IsNullOrWhiteSpace(appConnStr))
                {
                    options.UseNpgsql(appConnStr, b =>
                    {
                        b.MigrationsAssembly(assembly);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });

                    // Configure distributed cache
                    var cache = serviceProvider.GetRequiredService<IDistributedCache>();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.AddInterceptors(new CachingInterceptor(cache));
                }
            });
        }



        public static void AddPersistenceRepositories(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<IProductAttributeRepositoryAsync, ProductAttributeRepositoryAsync>();
            services.AddTransient<IProductAttributeMappingRepositoryAsync, ProductAttributeMappingRepositoryAsync>();
            services.AddTransient<IProductAttrValueRepositoryAsync, ProductAttrValueRepositoryAsync>();
            #endregion
        }
    }

    public class CachingInterceptor : DbCommandInterceptor
    {
        private readonly IDistributedCache _cache;

        public CachingInterceptor(IDistributedCache cache)
        {
            _cache = cache;
        }

        // Override methods to implement caching logic here
        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = GenerateCacheKey(command);
            var cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedResult))
            {
                // Deserialize the cached result and return it as a DbDataReader
                var dbDataReader = DeserializeResult(cachedResult);
                return InterceptionResult<DbDataReader>.SuppressWithResult(dbDataReader);
            }
            else
            {
                var dbResult = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
                // Serialize the dbResult and cache it
                await _cache.SetStringAsync(cacheKey, SerializeResult(dbResult.Result), cancellationToken);
                return dbResult;
            }
        }

        private string GenerateCacheKey(DbCommand command)
        {
            // Implement logic to generate a unique cache key based on the command
            return command.CommandText; // Simplified for illustration
        }

        private string SerializeResult(DbDataReader result)
        {
            // Implement serialization logic
            // This is a simplified example, you would need to properly serialize the reader's data
            return "serialized_result";
        }

        private DbDataReader DeserializeResult(string cachedResult)
        {
            // Implement deserialization logic
            // This is a simplified example, you would need to properly deserialize the cached data
            return null;
        }
    }
}
