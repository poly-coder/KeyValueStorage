using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobClientWrapperFactory : IBlobClientWrapperFactory
    {
        public IBlobClient CreateWrapper(BlobClient blobClient) =>
            new BlobClientWrapper(blobClient, this);
    }
}