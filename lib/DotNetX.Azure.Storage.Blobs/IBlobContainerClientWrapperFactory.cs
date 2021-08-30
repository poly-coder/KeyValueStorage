using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobContainerClientWrapperFactory
    {
        IBlobContainerClient CreateWrapper(BlobContainerClient blobContainerClient);
    }
}