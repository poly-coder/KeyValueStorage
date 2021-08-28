using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyValueMetadataStorer : 
        IKeyValueStorer
    {
        Task StoreMetadataAsync(
            object key, 
            object metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);

        Task StoreMetadataAndValueAsync(
            object key, 
            object value, 
            object metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyValueMetadataStorer<TKey, TValue, TMetadata> : 
        IKeyValueStorer<TKey, TValue>, 
        IKeyValueMetadataStorer
    {
        Task StoreMetadataAsync(
            TKey key, 
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);

        Task StoreMetadataAndValueAsync(
            TKey key, 
            TValue value,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);
    }
}