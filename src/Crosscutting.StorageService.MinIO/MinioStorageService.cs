using Crosscutting.StorageService.Base;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

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
            var bucket = new BucketExistsArgs().WithBucket(bucketName);

            if (!await _minioClient.BucketExistsAsync(bucket))
            {
                throw new MinioException($"buckectName {bucketName} not found");
            }

            var response =
                await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithExpiry(3600)
                );

            return response;
        }

        public Task<StorageResponseDTO> ReplaceAsync(string bucketName, string objectName, Stream dataStream, string contentType)
        {
            throw new NotImplementedException();
        }

        public async Task<StorageResponseDTO> UploadAsync(string bucketName, string objectName, Stream dataStream, string contentType)
        {
            var bucket = new BucketExistsArgs().WithBucket(bucketName);

            if (!await _minioClient.BucketExistsAsync(bucket))
            {
                var makeBucket = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucket);
            }

            try
            {
                var shouldReplace = await _minioClient.StatObjectAsync(new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    );
                if (shouldReplace != null)
                {
                    await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        );
                }
            }
            catch (ObjectNotFoundException ex) { };

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(dataStream)
                .WithObjectSize(dataStream.Length)
                .WithContentType(contentType);

            var response = await _minioClient.PutObjectAsync(putObjectArgs);
            return new StorageResponseDTO()
            {
                Etag = response.Etag,
                ObjectName = response.ObjectName,
                Size = response.Size,
                Url = $"{response.ObjectName}"
            };
        }
    }
}
