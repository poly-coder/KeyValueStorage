namespace DotNetX.Azure.Storage.Blobs.DependencyInjection
{
    public interface IBlobServiceClientSettings
    {
        string ConnectionString { get; set; }
    }
}