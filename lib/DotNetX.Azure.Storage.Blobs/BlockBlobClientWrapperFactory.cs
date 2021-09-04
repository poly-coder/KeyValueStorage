using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlockBlobClientWrapperFactory : IBlockBlobClientWrapperFactory
    {
        public IBlockBlobClient CreateWrapper(BlockBlobClient blockBlobClient) =>
            new BlockBlobClientWrapper(blockBlobClient, this);
    }
}