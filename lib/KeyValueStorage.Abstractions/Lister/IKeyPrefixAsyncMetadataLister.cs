using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixAsyncMetadataLister :
        IKeyPrefixAsyncLister,
        IKeyPrefixMetadataLister
    {
        IAsyncEnumerable<ICollection<KeyMetadataListerItem>> ListAsyncPrefixedMetadataKeys(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyPrefixAsyncMetadataLister<TKey, TMetadata> :
        IKeyPrefixAsyncLister<TKey>,
        IKeyPrefixMetadataLister<TKey, TMetadata>,
        IKeyPrefixAsyncMetadataLister
    {
        IAsyncEnumerable<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListAsyncPrefixedMetadataKeys(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}
