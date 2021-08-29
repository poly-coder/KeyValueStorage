using System;
using System.Collections;
using System.Collections.Generic;
using Azure;

namespace DotNetX.Azure.Storage.Blobs
{
    public class PageableWrapper<T> :
        IPageable<T>
        where T : notnull
    {
        private readonly Pageable<T> pageable;

        public PageableWrapper(Pageable<T> pageable)
        {
            this.pageable = pageable ?? throw new ArgumentNullException(nameof(pageable));
        }

        public IEnumerable<Page<T>> AsPages(string? continuationToken = default, int? pageSizeHint = default)
        {
            return pageable.AsPages(continuationToken, pageSizeHint);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return pageable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}