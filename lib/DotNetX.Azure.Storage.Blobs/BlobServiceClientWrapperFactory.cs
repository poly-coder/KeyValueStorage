using Azure.Storage.Blobs;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobServiceClientWrapperFactory : IBlobServiceClientWrapperFactory
    {
        private readonly IBlobContainerClientWrapperFactory containerFactory;
        private readonly IPageableWrapperFactory pageableFactory;
        private readonly IAsyncPageableWrapperFactory asyncPageableFactory;

        public BlobServiceClientWrapperFactory(
            IBlobContainerClientWrapperFactory containerFactory, 
            IPageableWrapperFactory pageableFactory, 
            IAsyncPageableWrapperFactory asyncPageableFactory)
        {
            this.containerFactory = containerFactory;
            this.pageableFactory = pageableFactory;
            this.asyncPageableFactory = asyncPageableFactory;
        }

        public IBlobServiceClient CreateWrapper(BlobServiceClient blobServiceClient) =>
            new BlobServiceClientWrapper(
                blobServiceClient, 
                containerFactory, 
                pageableFactory, 
                asyncPageableFactory);
    }
}
