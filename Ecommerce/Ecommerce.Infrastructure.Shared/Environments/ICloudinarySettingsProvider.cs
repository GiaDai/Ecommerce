using System;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public interface ICloudinarySettingsProvider
    {
        string GetConnectionString();
    }
}
