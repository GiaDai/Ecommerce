using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.Identity.Contexts;
using Ecommerce.Infrastructure.Identity.Models;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Serilog;

namespace Ecommerce.WebApp.FrontEnd.Server.Initializer
{
    public class ApplicationInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task InitializeAsync()
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            try
            {
                Log.Information("Application Starting");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "An error occurred seeding the DB");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            return Task.CompletedTask;
        }
    }
}
