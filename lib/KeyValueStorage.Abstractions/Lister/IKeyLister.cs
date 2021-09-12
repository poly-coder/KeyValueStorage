using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public record KeyListerItem(
        object Key);

    public record KeyListerItem<TKey>(
        TKey Key);

    public interface IKeyLister :
        IKeyValueStorage
    {
        Task<ICollection<KeyListerItem>> ListKeysAsync(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyLister<TKey> :
        IKeyLister
    {
        Task<ICollection<KeyListerItem<TKey>>> ListKeysAsync(
            CancellationToken cancellationToken = default);
    }
}
