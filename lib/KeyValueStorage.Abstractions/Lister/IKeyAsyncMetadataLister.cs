using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyAsyncMetadataLister :
        IKeyAsyncLister,
        IKeyMetadataLister
    {
        IAsyncEnumerable<ICollection<KeyMetadataListerItem>> ListAsyncMetadataKeys(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyAsyncMetadataLister<TKey, TMetadata> :
        IKeyAsyncLister<TKey>,
        IKeyMetadataLister<TKey, TMetadata>,
        IKeyAsyncMetadataLister
    {
        new IAsyncEnumerable<ICollection<KeyMetadataListerItem>> ListAsyncMetadataKeys(
            CancellationToken cancellationToken = default);
    }
}