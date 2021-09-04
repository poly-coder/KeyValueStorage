namespace DotNetX.Azure.Storage.Blobs.DependencyInjection
{
    public class BlobServiceClientSettings : IBlobServiceClientSettings
    {
        public string ConnectionString { get; set; }
    }
}