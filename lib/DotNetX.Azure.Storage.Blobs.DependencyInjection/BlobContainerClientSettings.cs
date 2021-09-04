namespace DotNetX.Azure.Storage.Blobs.DependencyInjection
{
    public class BlobContainerClientSettings :
        BlobServiceClientSettings,
        IBlobContainerClientSettings
    {
        public string Container { get; set; }
    }
}