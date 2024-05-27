
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ecommerce.Infrastructure.Shared.Environments;
using Newtonsoft.Json;

namespace Ecommerce.WebApp.Server.HostedService;

public class RecureHostedService : IHostedService, IDisposable
{
    private readonly ILogger _log;
    private Timer _timer;
    private readonly Cloudinary _cloudinary;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICloudinarySettingsProvider _cloudinaryProvider;
    public RecureHostedService(
        ILogger<RecureHostedService> log,
        IHttpClientFactory httpClientFactory,
        ICloudinarySettingsProvider cloudinaryProvider
        )
    {
        _log = log;
        _httpClientFactory = httpClientFactory;
        _cloudinaryProvider = cloudinaryProvider;
        _cloudinary = new Cloudinary(_cloudinaryProvider.GetConnectionString());
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("RecureHostedService is Starting");
        _log.LogInformation(_cloudinaryProvider.GetConnectionString());
        DownloadPolicyFromCloudinary(null); // Pass null as the argument

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        UploadPolicyToCloudinary(null); // Pass null as the argument
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void UploadPolicyToCloudinary(object? state)
    {
        // get file path policy.csv in wwwroot directory
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "policy.csv");
        if (File.Exists(filePath))
        {
            // upload file to cloudinary
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(filePath),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = "policy",
            };
            var uploadResult = _cloudinary.Upload(uploadParams);
            if (uploadResult.Error != null)
            {
                _log.LogError(uploadResult.Error.Message);
            }
            else
            {
                _log.LogInformation("Upload policy.csv to cloudinary successfully");
                _log.LogInformation(uploadResult.SecureUrl.AbsoluteUri);
            }
        }

    }

    private void DownloadPolicyFromCloudinary(object? state)
    {
        // download file policy/policy.csv from cloudinary and save to wwwroot directory
        var getResourceParams = new GetResourceParams("policy/policy.csv")
        {
            ResourceType = ResourceType.Raw,
        };
        var getResourceResult = _cloudinary.GetResource(getResourceParams);
        var resultJson = getResourceResult.JsonObj;
        var url = resultJson["url"]?.ToString();
        if (url != null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "policy.csv");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, content);
                _log.LogInformation("Download policy.csv from cloudinary successfully");
            }
        }
    }
}
