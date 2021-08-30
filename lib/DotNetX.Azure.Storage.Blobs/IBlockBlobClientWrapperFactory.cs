using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlockBlobClientWrapperFactory
    {
        IBlockBlobClient CreateWrapper(BlockBlobClient blockBlobClient);
    }
}