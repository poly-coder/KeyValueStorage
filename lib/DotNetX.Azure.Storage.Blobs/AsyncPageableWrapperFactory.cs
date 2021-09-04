using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public class AsyncPageableWrapperFactory : IAsyncPageableWrapperFactory
    {
        public IAsyncPageable<T> CreateWrapper<T>(AsyncPageable<T> pageable) =>
            new AsyncPageableWrapper<T>(pageable);
    }
}