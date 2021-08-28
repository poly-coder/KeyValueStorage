using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixLister :
        IKeyValueStorage
    {
        Task<ICollection<KeyListerItem>> ListPrefixedKeysAsync(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyPrefixLister<TKey> :
        IKeyPrefixLister
    {
        Task<ICollection<KeyListerItem<TKey>>> ListPrefixedKeysAsync(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}