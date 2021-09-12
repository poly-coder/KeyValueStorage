using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public abstract record KeyMetadataStoreEvent(object Key, object Metadata)
    {
        public record Created(object Key, object Metadata) : KeyMetadataStoreEvent(Key, Metadata);
        public record Replaced(object Key, object Metadata, bool ReplacedMetadata, bool ReplacedValue) :
            KeyMetadataStoreEvent(Key, Metadata);
        public record Removed(object Key, object Metadata) : KeyMetadataStoreEvent(Key, Metadata);
    }

    public abstract record KeyMetadataStoreEvent<TKey, TMetadata>(TKey Key, TMetadata Metadata)
    {
        public record Created(TKey Key, TMetadata Metadata) : KeyMetadataStoreEvent<TKey, TMetadata>(Key, Metadata);
        public record Replaced(TKey Key, TMetadata Metadata, bool ReplacedMetadata, bool ReplacedValue) :
            KeyMetadataStoreEvent<TKey, TMetadata>(Key, Metadata);
        public record Removed(TKey Key, TMetadata Metadata) : KeyMetadataStoreEvent<TKey, TMetadata>(Key, Metadata);
    }

    public interface IKeyMetadataStoreEvents :
        IKeyValueStorage
    {
        IAsyncEnumerable<ICollection<KeyMetadataStoreEvent>> SubscribeKeyMetadataEvents(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyMetadataStoreEvents<TKey, TMetadata> :
        IKeyMetadataStoreEvents
    {
        new IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<TKey, TMetadata>>> SubscribeKeyMetadataEvents(
            CancellationToken cancellationToken = default);
    }
}