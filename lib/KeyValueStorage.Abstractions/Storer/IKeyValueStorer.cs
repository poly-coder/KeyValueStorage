using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public enum KeyValueStoreMode
    {
        CreateNew = 1,
        ReplaceExisting = 2,
        CreateOrReplace = 3,
    }

    public interface IKeyValueStorer :
        IKeyValueStorage
    {
        Task StoreAsync(
            object key,
            object value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            object key,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyValueStorer<TKey, TValue> :
        IKeyValueStorer
    {
        Task StoreAsync(
            TKey key,
            TValue value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            TKey key,
            CancellationToken cancellationToken = default);
    }
}