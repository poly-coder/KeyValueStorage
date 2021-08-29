using System.Collections.Generic;
using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IPageable<T> :
        IEnumerable<T>
        where T : notnull
    {
        IEnumerable<Page<T>> AsPages(
            string? continuationToken = default,
            int? pageSizeHint = default);
    }
}