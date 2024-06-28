using System.Net.Sockets;
using Ecommerce.Application;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Shared;
using Ecommerce.WebApp.FrontEnd.Server.Extensions;
using Ecommerce.WebApp.FrontEnd.Server.Middlewares;
using Ecommerce.WebApp.FrontEnd.Server.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;
var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;
var _services = builder.Services;
var _env = builder.Environment;
// Add services to the container. Trigger Githun Actions

//Read Configuration from appSettings
var configSerilog = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();
//Initialize Logger
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information) // Log EF Core SQL
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .WriteTo.Udp(
        remoteAddress: "localhost",  // Thay 'logstash-host' bằng địa chỉ IP hoặc hostname của Logstash
        remotePort: 5044,
        family: AddressFamily.InterNetwork,
        localPort: 0,
        enableBroadcast: false,
        restrictedToMinimumLevel: LogEventLevel.Verbose,
        levelSwitch: null,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
        formatProvider: null,
        encoding: null
    )
    .CreateLogger();
builder.Host.UseSerilog();
_services.AddEnvironmentVariablesExtension();
_services.AddIdentityLayer();
_services.AddApplicationLayer();
_services.AddNpgSqlIdentityInfrastructure();
_services.AddIdentityRepositories(_config);
_services.AddNpgSqlPersistenceInfrastructure(typeof(Program).Assembly.FullName);
_services.AddNpgSqlCQRSPersistenceInfrastructure(typeof(Program).Assembly.FullName);
_services.AddPersistenceRepositories();
_services.AddSharedInfrastructure(_config);
if (_env.IsDevelopment())
{
    _services.AddSwaggerExtension();
}

_services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
_services.AddApiVersioningExtension();
_services.AddHealthChecks();
_services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
_services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
_services.AddEndpointsApiExplorer();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (_env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseAuthorization();
app.UseSerilogRequestLogging(); // Add this line
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");
// config readliness check with path /ready
app.UseHealthChecks("/ready", new HealthCheckOptions()
{
    Predicate = (check) => check.Name == "readiness"
});
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
