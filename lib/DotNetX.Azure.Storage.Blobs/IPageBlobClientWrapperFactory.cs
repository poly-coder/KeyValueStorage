using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IPageBlobClientWrapperFactory
    {
        IPageBlobClient CreateWrapper(PageBlobClient pageBlobClient);
    }
}