using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IAppendBlobClientWrapperFactory
    {
        IAppendBlobClient CreateWrapper(AppendBlobClient appendBlobClient);
    }
}