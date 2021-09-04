using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public class AppendBlobClientWrapperFactory : IAppendBlobClientWrapperFactory
    {
        public IAppendBlobClient CreateWrapper(AppendBlobClient appendBlobClient) =>
            new AppendBlobClientWrapper(appendBlobClient, this);
    }
}