using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public class PageBlobClientWrapperFactory : IPageBlobClientWrapperFactory
    {
        public IPageBlobClient CreateWrapper(PageBlobClient pageBlobClient) =>
            new PageBlobClientWrapper(pageBlobClient, this);
    }
}