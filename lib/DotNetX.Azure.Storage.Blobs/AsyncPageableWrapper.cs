using System;
using System.Collections.Generic;
using System.Threading;
using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public class AsyncPageableWrapper<T> : 
        IAsyncPageable<T> 
        where T : notnull
    {
        private readonly AsyncPageable<T> pageable;

        public AsyncPageableWrapper(AsyncPageable<T> pageable)
        {
            this.pageable = pageable ?? throw new ArgumentNullException(nameof(pageable));
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return pageable.GetAsyncEnumerator(cancellationToken);
        }

        public IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = default, int? pageSizeHint = default)
        {
            return pageable.AsPages(continuationToken, pageSizeHint);
        }
    }
}