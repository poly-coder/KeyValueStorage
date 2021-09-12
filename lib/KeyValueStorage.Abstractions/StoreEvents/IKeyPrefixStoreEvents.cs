using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixStoreEvents :
        IKeyStoreEvents
    {
        IAsyncEnumerable<ICollection<KeyStoreEvent>> SubscribeKeyPrefixEvents(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyPrefixStoreEvents<TKey> :
        IKeyPrefixStoreEvents,
        IKeyStoreEvents<TKey>
    {
        IAsyncEnumerable<ICollection<KeyStoreEvent<TKey>>> SubscribeKeyPrefixEvents(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}