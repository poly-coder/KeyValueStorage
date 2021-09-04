using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IPageableWrapperFactory
    {
        IPageable<T> CreateWrapper<T>(Pageable<T> pageable);
    }
}