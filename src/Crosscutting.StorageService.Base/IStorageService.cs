namespace Crosscutting.StorageService.Base
{
    public interface IStorageService
    {
        Task<string> GetAsync(string bucketName, string name);
        Task<StorageResponseDTO> UploadAsync(string bucketName, string name, Stream dataStream, string contentType);
        Task DeleteAsync(string bucketName, string name);
    }
}
