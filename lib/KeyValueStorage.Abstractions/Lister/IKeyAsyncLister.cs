using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyAsyncLister :
        IKeyLister
    {
        IAsyncEnumerable<ICollection<KeyListerItem>> ListAsyncKeys(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyAsyncLister<TKey> :
        IKeyLister<TKey>,
        IKeyAsyncLister
    {
        new IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> ListAsyncKeys(
            CancellationToken cancellationToken = default);
    }
}