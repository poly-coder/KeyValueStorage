using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixMetadataStoreEvents :
        IKeyPrefixStoreEvents,
        IKeyMetadataStoreEvents
    {
        IAsyncEnumerable<ICollection<KeyMetadataStoreEvent>> SubscribeKeyPrefixMetadataEvents(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyPrefixMetadataStoreEvents<TKey, TMetadata> :
        IKeyPrefixMetadataStoreEvents,
        IKeyPrefixStoreEvents<TKey>,
        IKeyMetadataStoreEvents<TKey, TMetadata>
    {
        IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<TKey, TMetadata>>> SubscribeKeyPrefixMetadataEvents(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}