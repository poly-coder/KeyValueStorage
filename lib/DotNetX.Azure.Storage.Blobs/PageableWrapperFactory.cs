using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public class PageableWrapperFactory : IPageableWrapperFactory
    {
        public IPageable<T> CreateWrapper<T>(Pageable<T> pageable) =>
            new PageableWrapper<T>(pageable);
    }
}