using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IAsyncPageableWrapperFactory
    {
        IAsyncPageable<T> CreateWrapper<T>(AsyncPageable<T> pageable);
    }
}