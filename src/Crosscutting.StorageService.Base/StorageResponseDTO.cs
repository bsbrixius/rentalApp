namespace Crosscutting.StorageService.Base
{
    public class StorageResponseDTO
    {
        public string Url { get; set; }
        public string Etag { get; set; }
        public string ObjectName { get; set; }
        public long Size { get; set; }
    }
}
