using Crosscutting.StorageService.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Crosscutting.StorageService.MinIO
{
    public static class Configuration
    {
        public static void AddMinIOStorageService(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MinIOSettings>(options => configuration.GetSection("MinIOSettings"));

            var endpoint = configuration.GetSection("MinIOSettings:Endpoint").Value;
            int.TryParse(configuration.GetSection("MinIOSettings:Port").Value, out var port);
            var accessKey = configuration.GetSection("MinIOSettings:AccessKey").Value;
            var secretKey = configuration.GetSection("MinIOSettings:SecretKey").Value;

            if (port == 0)
                throw new ArgumentException("Port must be a valid number");
            ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));
            ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));
            ArgumentNullException.ThrowIfNull(accessKey, nameof(accessKey));
            ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(endpoint, port)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build());
            services.AddSingleton<IStorageService, MinioStorageService>();
        }
    }
}
