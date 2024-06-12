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
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string appConnStr = _dbSetting.GetPostgresConnectionString();
                if (!string.IsNullOrWhiteSpace(appConnStr))
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                    appConnStr,
                    b =>
                    {
                        b.MigrationsAssembly(assembly);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));
                }
            }
        }

        public static void AddNpgSqlCQRSPersistenceInfrastructure(this IServiceCollection services, string assembly)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string readConnStr = _dbSetting.GetPostgresReadConnString();
                if (!string.IsNullOrWhiteSpace(readConnStr))
                {
                    services.AddDbContext<ReadDbContext>(options =>
                    options.UseNpgsql(
                    readConnStr,
                    b =>
                    {
                        b.MigrationsAssembly(assembly);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));
                }

                string writeConnStr = _dbSetting.GetPostgresWriteConnString();
                if (!string.IsNullOrWhiteSpace(writeConnStr))
                {
                    services.AddDbContext<WriteDbContext>(options =>
                    options.UseNpgsql(
                    writeConnStr,
                    b =>
                    {
                        b.MigrationsAssembly(assembly);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));
                }
            }
        }

        public static void AddPersistenceRepositories(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient(typeof(IGenericRepositoryBaseAsync<>), typeof(GenericRepositoryBaseAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<IProductAttributeRepositoryAsync, ProductAttributeRepositoryAsync>();
            services.AddTransient<IProductAttributeMappingRepositoryAsync, ProductAttributeMappingRepositoryAsync>();
            services.AddTransient<IProductAttrValueRepositoryAsync, ProductAttrValueRepositoryAsync>();
            #endregion
        }
    }
}
