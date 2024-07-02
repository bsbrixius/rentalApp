using Crosscutting.StorageService.Base;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Crosscutting.StorageService.MinIO
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinIOSettings _minIOSettings;

        public MinioStorageService(
            IMinioClient minioClient,
            IOptions<MinIOSettings> minIOSettings
            )
        {
            ArgumentNullException.ThrowIfNull(minioClient, nameof(minioClient));
            ArgumentNullException.ThrowIfNull(minIOSettings.Value, nameof(minIOSettings));
            _minioClient = minioClient;
            _minIOSettings = minIOSettings.Value;
        }
        public async Task DeleteAsync(string bucketName, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAsync(string bucketName, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<StorageResponseDTO> UploadAsync(string bucketName, string name, Stream dataStream, string contentType)
        {
            var bucket = new BucketExistsArgs().WithBucket(bucketName);

            if (await _minioClient.BucketExistsAsync(bucket))
            {
                var makeBucket = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucket);
            }
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithStreamData(dataStream)
                .WithContentType(contentType);
            var response = await _minioClient.PutObjectAsync(putObjectArgs);
            return new StorageResponseDTO()
            {
                Etag = response.Etag,
                ObjectName = response.ObjectName,
                Size = response.Size,
                Url = $"{_minIOSettings.Endpoint}/{bucketName}/{response.ObjectName}"
            };
        }
    }
}
