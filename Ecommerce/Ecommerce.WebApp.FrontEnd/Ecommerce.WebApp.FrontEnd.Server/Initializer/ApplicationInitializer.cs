using System.Net.Sockets;
using Serilog;
using Serilog.Events;

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
                .AddJsonFile("appsettings.Development.json")
                .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                // .ReadFrom.Configuration(config)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                .WriteTo.Udp(
                    remoteAddress: "localhost",  // Thay 'logstash-host' bằng địa chỉ IP hoặc hostname của Logstash
                    remotePort: 5044,
                    family: AddressFamily.InterNetwork,
                    localPort: 0,
                    enableBroadcast: false,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
                    levelSwitch: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
                    formatProvider: null,
                    encoding: null
                )
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
