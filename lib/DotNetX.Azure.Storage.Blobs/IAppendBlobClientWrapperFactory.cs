using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IAppendBlobClientWrapperFactory
    {
        IAppendBlobClient CreateWrapper(AppendBlobClient appendBlobClient);
    }

    public interface IBlobServiceClientWrapperFactory
    {
        IBlobServiceClient CreateWrapper(BlobServiceClient blobServiceClient);
    }
}