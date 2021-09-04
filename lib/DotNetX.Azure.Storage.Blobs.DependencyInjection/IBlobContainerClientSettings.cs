namespace DotNetX.Azure.Storage.Blobs.DependencyInjection
{
    public interface IBlobContainerClientSettings : IBlobServiceClientSettings
    {
        string Container { get; set; }
    }
}