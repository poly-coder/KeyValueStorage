using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixAsyncLister :
        IKeyPrefixLister
    {
        IAsyncEnumerable<ICollection<KeyListerItem>> ListAsyncPrefixedKeys(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }
 
    public interface IKeyPrefixAsyncLister<TKey> :
        IKeyPrefixAsyncLister
    {
        IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> ListAsyncPrefixedKeys(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}