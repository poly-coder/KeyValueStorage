using DotNetX.Threading;
using KeyValueStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KeyValueStorage.InMemory
{
    public record InMemoryKeyValueStorageEntry<TKey, TValue, TMetadata>(
        TKey Key, TValue Value, TMetadata Metadata);

    public class InMemoryKeyValueStorage<TKey, TValue, TMetadata> :
        LocalKeyValueStorageBase<TKey, TValue, TMetadata>
        where TKey : notnull
    {
        private readonly MailboxActorInterface<Command> actor;

        private const KeyValueStorageCapability Capabilities =
            KeyValueStorageCapability.Fetch |
            KeyValueStorageCapability.List |
            KeyValueStorageCapability.Store |
            KeyValueStorageCapability.Metadata;

        public InMemoryKeyValueStorage(
            IEnumerable<InMemoryKeyValueStorageEntry<TKey, TValue, TMetadata>>? entries = null,
            IEqualityComparer<TKey>? keyComparer = null)
            : base(Capabilities)
        {
            this.actor = MailboxActor.StartBounded(
                new BoundedChannelOptions(32)
                {
                    AllowSynchronousContinuations = false,
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = true,
                    SingleWriter = false,
                },
                CreateActorLoop(entries, keyComparer));
        }

        #region [ IKeyValueMetadataFetcher ]

        public override async Task<KeyValueFetchResponse<TValue>> FetchAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            var result = await actor.Mailbox
                .PostAndReplyAsync<Command, KeyValueMetadataFetchResponse<TValue, TMetadata>>(
                    reply => new Command.Fetch(key, reply),
                    cancellationToken: cancellationToken);

            return new KeyValueFetchResponse<TValue>(
                result.Exists, result.Value);
        }

        public override async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            var result = await actor.Mailbox
                .PostAndReplyAsync<Command, KeyValueMetadataFetchResponse<TValue, TMetadata>>(
                    reply => new Command.Fetch(key, reply),
                    cancellationToken: cancellationToken);

            return new KeyValueMetadataFetchResponse<TValue, TMetadata>(
                result.Exists, default!, result.Metadata);
        }

        public override async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAndValueAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            var result = await actor.Mailbox
                .PostAndReplyAsync<Command, KeyValueMetadataFetchResponse<TValue, TMetadata>>(
                    reply => new Command.Fetch(key, reply),
                    cancellationToken: cancellationToken);

            return result;
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public override async Task StoreAsync(
            TKey key,
            TValue value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await actor.Mailbox
                .PostAndReplyAsync(
                    reply => new Command.Store(key, true, value, false, default!, storeMode, reply),
                    cancellationToken: cancellationToken);
        }

        public override async Task RemoveAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await actor.Mailbox
                .PostAndReplyAsync(
                    reply => new Command.Remove(key, reply),
                    cancellationToken: cancellationToken);
        }

        public override async Task StoreMetadataAsync(
            TKey key,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await actor.Mailbox
                .PostAndReplyAsync(
                    reply => new Command.Store(key, false, default!, true, metadata, storeMode, reply),
                    cancellationToken: cancellationToken);
        }

        public override async Task StoreMetadataAndValueAsync(
            TKey key,
            TValue value,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await actor.Mailbox
                .PostAndReplyAsync(
                    reply => new Command.Store(key, true, value, true, metadata, storeMode, reply),
                    cancellationToken: cancellationToken);
        }
        #endregion

        #region [ IKeyAsyncMetadataLister ]

        public override async Task<ICollection<KeyListerItem<TKey>>> ListKeysAsync(CancellationToken cancellationToken)
        {
            var result = await actor.Mailbox
                .PostAndReplyAsync<Command, ICollection<KeyMetadataListerItem<TKey, TMetadata>>>(
                    reply => new Command.ListKeys(reply),
                    cancellationToken: cancellationToken);

            return result
                .Select(e => new KeyListerItem<TKey>(e.Key))
                .ToList();
        }

        public override async Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListMetadataKeysAsync(CancellationToken cancellationToken)
        {
            var result = await actor.Mailbox
                .PostAndReplyAsync<Command, ICollection<KeyMetadataListerItem<TKey, TMetadata>>>(
                    reply => new Command.ListKeys(reply),
                    cancellationToken: cancellationToken);

            return result;
        }

        #endregion

        #region [ MailboxActor ]

        private abstract record Command
        {
            public record Fetch(
                TKey Key,
                TaskCompletionSource<KeyValueMetadataFetchResponse<TValue, TMetadata>> Reply) : Command;

            public record Store(
                TKey Key,
                bool StoreValue,
                TValue Value,
                bool StoreMetadata,
                TMetadata Metadata,
                KeyValueStoreMode StoreMode,
                TaskCompletionSource Reply) : Command;

            public record Remove(
                TKey Key,
                TaskCompletionSource Reply) : Command;

            public record ListKeys(
                TaskCompletionSource<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> Reply) : Command;
        }

        private Func<ChannelReader<Command>, Task> CreateActorLoop(
            IEnumerable<InMemoryKeyValueStorageEntry<TKey, TValue, TMetadata>>? entries = null,
            IEqualityComparer<TKey>? keyComparer = null)
        {
            entries ??= Enumerable.Empty<InMemoryKeyValueStorageEntry<TKey, TValue, TMetadata>>();
            keyComparer ??= EqualityComparer<TKey>.Default;

            async Task ActorLoop(ChannelReader<Command> reader)
            {
                var data = new Dictionary<TKey, (TValue value, TMetadata meta)>(
                    entries.Select(e => KeyValuePair.Create(e.Key, (e.Value, e.Metadata))),
                    keyComparer);

                await foreach (var command in reader.ReadAllAsync())
                {
                    switch (command)
                    {
                        case Command.Fetch(var key, var reply):
                            OnFetch(data, key, reply);
                            break;

                        case Command.Store(var key, var storeValue, var value, 
                            var storeMetadata, var metadata, var storeMode, var reply):
                            OnStore(data, key, storeValue, value, storeMetadata, metadata, storeMode, reply);
                            break;

                        case Command.Remove(var key, var reply):
                            OnRemove(data, key, reply);
                            break;

                        case Command.ListKeys(var reply):
                            OnListKeys(data, reply);
                            break;
                    }
                }

                static void OnFetch(
                    Dictionary<TKey, (TValue value, TMetadata meta)> data,
                    TKey key,
                    TaskCompletionSource<KeyValueMetadataFetchResponse<TValue, TMetadata>> reply)
                {
                    if (data.TryGetValue(key, out var pair))
                    {
                        reply.TrySetResult(
                            new KeyValueMetadataFetchResponse<TValue, TMetadata>(
                                true, pair.value, pair.meta));
                    }
                    else
                    {
                        reply.TrySetResult(
                            new KeyValueMetadataFetchResponse<TValue, TMetadata>(
                                false, default!, default!));
                    }
                }

                static void OnStore(
                    Dictionary<TKey, (TValue value, TMetadata meta)> data,
                    TKey key,
                    bool storeValue,
                    TValue? value,
                    bool storeMetadata,
                    TMetadata? metadata,
                    KeyValueStoreMode storeMode,
                    TaskCompletionSource reply)
                {
                    if (data.TryGetValue(key, out var pair))
                    {
                        switch (storeMode)
                        {
                            case KeyValueStoreMode.CreateNew:
                                {
                                    reply.TrySetException(
                                        new InvalidOperationException($"Key '{key}' already exists"));
                                    break;
                                }

                            default:
                                {
                                    var newValue = storeValue ? value : pair.value;
                                    var newMetadata = storeMetadata ? metadata : pair.meta;
                                    data[key] = (newValue!, newMetadata!);
                                    reply.TrySetResult();
                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (storeMode)
                        {
                            case KeyValueStoreMode.ReplaceExisting:
                                {
                                    reply.TrySetException(
                                        new InvalidOperationException($"Key '{key}' does not exist"));
                                    break;
                                }
                            case KeyValueStoreMode.CreateNew:
                                {
                                    if (!storeValue)
                                    {
                                        reply.TrySetException(
                                            new InvalidOperationException($"Value is required when creating a new key value pair, for key '{key}'"));
                                    }
                                    else
                                    {
                                        var newValue = storeValue ? value : default!;
                                        var newMetadata = storeMetadata ? metadata : default!;
                                        data[key] = (newValue!, newMetadata!);
                                        reply.TrySetResult();
                                    }
                                    break;
                                }
                            case KeyValueStoreMode.CreateOrReplace:
                                {
                                    var newValue = storeValue ? value : default;
                                    var newMetadata = storeMetadata ? metadata : default;
                                    data[key] = (newValue!, newMetadata!);
                                    reply.TrySetResult();
                                    break;
                                }
                        }
                    }
                }

                static void OnRemove(
                    Dictionary<TKey, (TValue value, TMetadata meta)> data,
                    TKey key,
                    TaskCompletionSource reply)
                {
                    data.Remove(key);
                    reply.TrySetResult();
                }

                static void OnListKeys(
                    Dictionary<TKey, (TValue value, TMetadata meta)> data, 
                    TaskCompletionSource<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> reply)
                {
                    var result = data
                        .Select(kv => new KeyMetadataListerItem<TKey, TMetadata>(kv.Key, kv.Value.meta))
                        .ToList();

                    reply.TrySetResult(result);
                }
            }

            return ActorLoop;
        }

        #endregion
    }
}
