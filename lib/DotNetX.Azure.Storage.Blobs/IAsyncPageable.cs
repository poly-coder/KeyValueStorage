using System.Collections.Generic;
using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IAsyncPageable<T> :
        IAsyncEnumerable<T>
        where T : notnull
    {
        IAsyncEnumerable<Page<T>> AsPages(
            string? continuationToken = default,
            int? pageSizeHint = default);
    }
}