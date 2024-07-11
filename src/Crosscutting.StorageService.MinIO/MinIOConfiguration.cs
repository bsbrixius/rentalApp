using Crosscutting.StorageService.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using System.Net;

namespace Crosscutting.StorageService.MinIO
{
    public static class MinIOConfiguration
    {
        public static void AddMinIOStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinIOSettings>(options =>
            {
                options.Endpoint = configuration.GetSection("MinIOPlaygroundSettings:Endpoint").Value;
                options.AccessKey = configuration.GetSection("MinIOPlaygroundSettings:AccessKey").Value;
                options.SecretKey = configuration.GetSection("MinIOPlaygroundSettings:SecretKey").Value;
                options.ExternalUrl = configuration.GetSection("MinIOPlaygroundSettings:ExternalUrl").Value;
            });

            var endpoint = configuration.GetSection("MinIOSettings:Endpoint").Value;
            var accessKey = configuration.GetSection("MinIOSettings:AccessKey").Value;
            var secretKey = configuration.GetSection("MinIOSettings:SecretKey").Value;

            ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));
            ArgumentNullException.ThrowIfNull(accessKey, nameof(accessKey));
            ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build());
            services.AddSingleton<IStorageService, MinioStorageService>();
        }

        public static void AddMinIOPlaygroundStorageService(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MinIOSettings>(options =>
            {
                options.Endpoint = configuration.GetSection("MinIOPlaygroundSettings:Endpoint").Value;
                options.AccessKey = configuration.GetSection("MinIOPlaygroundSettings:AccessKey").Value;
                options.SecretKey = configuration.GetSection("MinIOPlaygroundSettings:SecretKey").Value;
                options.ExternalUrl = configuration.GetSection("MinIOPlaygroundSettings:ExternalUrl").Value;
            });

            var endpoint = configuration.GetSection("MinIOPlaygroundSettings:Endpoint").Value;
            var accessKey = configuration.GetSection("MinIOPlaygroundSettings:AccessKey").Value;
            var secretKey = configuration.GetSection("MinIOPlaygroundSettings:SecretKey").Value;

            ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoint));
            ArgumentNullException.ThrowIfNull(accessKey, nameof(accessKey));
            ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            services.AddMinio(configureClient => configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL()
                .Build());
            services.AddSingleton<IStorageService, MinioStorageService>();
        }
    }
}
