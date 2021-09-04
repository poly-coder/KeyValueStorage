using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobServiceClientWrapperFactory
    {
        IBlobServiceClient CreateWrapper(BlobServiceClient blobServiceClient);
    }
}
