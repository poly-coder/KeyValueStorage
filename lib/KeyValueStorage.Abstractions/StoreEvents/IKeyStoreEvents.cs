using System.Collections.Generic;
using System.Threading;

namespace KeyValueStorage.Abstractions
{
    public abstract record KeyStoreEvent(object Key)
    {
        public record Created(object Key) : KeyStoreEvent(Key);
        public record Replaced(object Key) : KeyStoreEvent(Key);
        public record Removed(object Key) : KeyStoreEvent(Key);
    }

    public abstract record KeyStoreEvent<TKey>(TKey Key)
    {
        public record Created(TKey Key) : KeyStoreEvent<TKey>(Key);
        public record Replaced(TKey Key) : KeyStoreEvent<TKey>(Key);
        public record Removed(TKey Key) : KeyStoreEvent<TKey>(Key);
    }

    public interface IKeyStoreEvents :
        IKeyValueStorage
    {
        IAsyncEnumerable<ICollection<KeyStoreEvent>> SubscribeKeyEvents(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyStoreEvents<TKey> :
        IKeyStoreEvents
    {
        new IAsyncEnumerable<ICollection<KeyStoreEvent<TKey>>> SubscribeKeyEvents(
            CancellationToken cancellationToken = default);
    }
}
