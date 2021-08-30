using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobClientWrapperFactory
    {
        IBlobClient CreateWrapper(BlobClient blobClient);
    }
}