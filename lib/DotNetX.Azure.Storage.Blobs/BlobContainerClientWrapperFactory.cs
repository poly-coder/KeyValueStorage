using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobContainerClientWrapperFactory : IBlobContainerClientWrapperFactory
    {
        private readonly IBlobClientWrapperFactory blobClientFactory;
        private readonly IBlockBlobClientWrapperFactory blockBlobClientFactory;
        private readonly IAppendBlobClientWrapperFactory appendBlobClientFactory;
        private readonly IPageBlobClientWrapperFactory pageBlobClientFactory;
        private readonly IPageableWrapperFactory pageableFactory;
        private readonly IAsyncPageableWrapperFactory asyncPageableFactory;

        public BlobContainerClientWrapperFactory(
            IBlobClientWrapperFactory blobClientFactory,
            IBlockBlobClientWrapperFactory blockBlobClientFactory,
            IAppendBlobClientWrapperFactory appendBlobClientFactory,
            IPageBlobClientWrapperFactory pageBlobClientFactory,
            IPageableWrapperFactory pageableFactory,
            IAsyncPageableWrapperFactory asyncPageableFactory)
        {
            this.blobClientFactory = blobClientFactory;
            this.blockBlobClientFactory = blockBlobClientFactory;
            this.appendBlobClientFactory = appendBlobClientFactory;
            this.pageBlobClientFactory = pageBlobClientFactory;
            this.pageableFactory = pageableFactory;
            this.asyncPageableFactory = asyncPageableFactory;
        }

        public IBlobContainerClient CreateWrapper(BlobContainerClient blobContainerClient) =>
            new BlobContainerClientWrapper(
                blobContainerClient,
                blobClientFactory,
                blockBlobClientFactory,
                appendBlobClientFactory,
                pageBlobClientFactory,
                pageableFactory,
                asyncPageableFactory);
    }
}