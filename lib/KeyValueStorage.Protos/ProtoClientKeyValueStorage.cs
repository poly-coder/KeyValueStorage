using Google.Protobuf;
using KeyValueStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Metadata = System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>>;

namespace KeyValueStorage.Protos
{
    // TODO: Catch RpcException to handle errors
    // TODO: Pass a Credentials provider in case auth is required
    // TODO: Pass OpenTelemetry headers and create StartActivity when a call is received with corresponding headers
    public class ProtoClientKeyValueStorage :
        KeyValueStorageBase<string, byte[], Metadata>

    {
        private readonly KeyValueStorage.KeyValueStorageClient keyValueStorageClient;
        private readonly KeyValueFetcher.KeyValueFetcherClient? keyValueFetcherClient;
        private readonly KeyValueMetadataFetcher.KeyValueMetadataFetcherClient? keyValueMetadataFetcherClient;
        private readonly KeyValueStorer.KeyValueStorerClient? keyValueStorerClient;
        private readonly KeyValueMetadataStorer.KeyValueMetadataStorerClient? keyValueMetadataStorerClient;
        private readonly KeyLister.KeyListerClient? keyListerClient;
        private readonly KeyMetadataLister.KeyMetadataListerClient? keyMetadataListerClient;
        private readonly KeyAsyncLister.KeyAsyncListerClient? keyAsyncListerClient;
        private readonly KeyAsyncMetadataLister.KeyAsyncMetadataListerClient? keyAsyncMetadataListerClient;
        private readonly KeyPrefixLister.KeyPrefixListerClient? keyPrefixListerClient;
        private readonly KeyPrefixMetadataLister.KeyPrefixMetadataListerClient? keyPrefixMetadataListerClient;
        private readonly KeyPrefixAsyncLister.KeyPrefixAsyncListerClient? keyPrefixAsyncListerClient;
        private readonly KeyPrefixAsyncMetadataLister.KeyPrefixAsyncMetadataListerClient? keyPrefixAsyncMetadataListerClient;
        private readonly KeyStoreEvents.KeyStoreEventsClient? keyStoreEventsClient;
        private readonly KeyMetadataStoreEvents.KeyMetadataStoreEventsClient? keyMetadataStoreEventsClient;
        private readonly KeyPrefixStoreEvents.KeyPrefixStoreEventsClient? keyPrefixStoreEventsClient;
        private readonly KeyPrefixMetadataStoreEvents.KeyPrefixMetadataStoreEventsClient? keyPrefixMetadataStoreEventsClient;

        public ProtoClientKeyValueStorage(
            KeyValueStorage.KeyValueStorageClient keyValueStorageClient,
            KeyValueFetcher.KeyValueFetcherClient? keyValueFetcherClient = null,
            KeyValueMetadataFetcher.KeyValueMetadataFetcherClient? keyValueMetadataFetcherClient = null,
            KeyValueStorer.KeyValueStorerClient? keyValueStorerClient = null,
            KeyValueMetadataStorer.KeyValueMetadataStorerClient? keyValueMetadataStorerClient = null,
            KeyLister.KeyListerClient? keyListerClient = null,
            KeyMetadataLister.KeyMetadataListerClient? keyMetadataListerClient = null,
            KeyAsyncLister.KeyAsyncListerClient? keyAsyncListerClient = null,
            KeyAsyncMetadataLister.KeyAsyncMetadataListerClient? keyAsyncMetadataListerClient = null,
            KeyPrefixLister.KeyPrefixListerClient? keyPrefixListerClient = null,
            KeyPrefixMetadataLister.KeyPrefixMetadataListerClient? keyPrefixMetadataListerClient = null,
            KeyPrefixAsyncLister.KeyPrefixAsyncListerClient? keyPrefixAsyncListerClient = null,
            KeyPrefixAsyncMetadataLister.KeyPrefixAsyncMetadataListerClient? keyPrefixAsyncMetadataListerClient = null,
            KeyStoreEvents.KeyStoreEventsClient? keyStoreEventsClient = null,
            KeyMetadataStoreEvents.KeyMetadataStoreEventsClient? keyMetadataStoreEventsClient = null,
            KeyPrefixStoreEvents.KeyPrefixStoreEventsClient? keyPrefixStoreEventsClient = null,
            KeyPrefixMetadataStoreEvents.KeyPrefixMetadataStoreEventsClient? keyPrefixMetadataStoreEventsClient = null
            )
        {
            this.keyValueStorageClient = keyValueStorageClient;
            this.keyValueFetcherClient = keyValueFetcherClient;
            this.keyValueMetadataFetcherClient = keyValueMetadataFetcherClient;
            this.keyValueStorerClient = keyValueStorerClient;
            this.keyValueMetadataStorerClient = keyValueMetadataStorerClient;
            this.keyListerClient = keyListerClient;
            this.keyMetadataListerClient = keyMetadataListerClient;
            this.keyAsyncListerClient = keyAsyncListerClient;
            this.keyAsyncMetadataListerClient = keyAsyncMetadataListerClient;
            this.keyPrefixListerClient = keyPrefixListerClient;
            this.keyPrefixMetadataListerClient = keyPrefixMetadataListerClient;
            this.keyPrefixAsyncListerClient = keyPrefixAsyncListerClient;
            this.keyPrefixAsyncMetadataListerClient = keyPrefixAsyncMetadataListerClient;
            this.keyStoreEventsClient = keyStoreEventsClient;
            this.keyMetadataStoreEventsClient = keyMetadataStoreEventsClient;
            this.keyPrefixStoreEventsClient = keyPrefixStoreEventsClient;
            this.keyPrefixMetadataStoreEventsClient = keyPrefixMetadataStoreEventsClient;
        }

        #region [ IKeyValueStorage ]

        private async Task CheckCapability(
            KeyValueStorageCapability capability,
            string name,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            bool isClientNull,
            CancellationToken cancellationToken)
        {
            if (isClientNull is true)
            {
                throw new NotSupportedException($"No corresponding client for capability {name} was provided");
            }

            var capabilities = await GetCapabilitiesAsync(cancellationToken);

            if (!capabilities.HasFlag(capability))
            {
                throw new NotImplementedException($"Remote provider does not implement capability {name}");
            }
        }

        protected override async Task<KeyValueStorageCapability> GetCapabilitiesOverride(
            CancellationToken cancellationToken)
        {
            var request = new GetCapabilitiesRequest();

            var response = await keyValueStorageClient.GetCapabilitiesAsync(
                request,
                cancellationToken: cancellationToken);

            return ToProperFlags(response);

            static KeyValueStorageCapability ToProperFlags(
                GetCapabilitiesResponse response)
            {
                var flags = KeyValueStorageCapability.None;

                if (response.Fetch)
                {
                    flags |= KeyValueStorageCapability.Fetch;
                }

                if (response.Store)
                {
                    flags |= KeyValueStorageCapability.Store;
                }

                if (response.List)
                {
                    flags |= KeyValueStorageCapability.List;
                }

                if (response.AsyncList)
                {
                    flags |= KeyValueStorageCapability.AsyncList;
                }

                if (response.KeyPrefix)
                {
                    flags |= KeyValueStorageCapability.KeyPrefix;
                }

                if (response.Metadata)
                {
                    flags |= KeyValueStorageCapability.Metadata;
                }

                if (response.StoreEvents)
                {
                    flags |= KeyValueStorageCapability.StoreEvents;
                }

                return flags;
            }
        }

        #endregion

        #region [ IKeyValueFetcher ]

        public override async Task<KeyValueFetchResponse<byte[]>> FetchAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch,
                "Fetch",
                keyValueFetcherClient is null,
                cancellationToken);

            var request = new FetchRequest()
            {
                Key = key,
            };

            var response = await keyValueFetcherClient!.FetchAsync(
                request, cancellationToken: cancellationToken);

            if (response.Exists)
            {
                return new KeyValueFetchResponse<byte[]>(
                    true,
                    response.Value.ToByteArray());
            }

            return new KeyValueFetchResponse<byte[]>(false, default!);
        }

        #endregion

        #region [ IKeyValueMetadataFetcher ]

        public override async Task<KeyValueMetadataFetchResponse<byte[], Metadata>> FetchMetadataAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata",
                keyValueMetadataFetcherClient is null,
                cancellationToken);

            var request = new FetchMetadataRequest()
            {
                Key = key,
            };

            var response = await keyValueMetadataFetcherClient!.FetchMetadataAsync(
                request, cancellationToken: cancellationToken);

            if (response.Exists)
            {
                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    true,
                    default!,
                    response.Metadata.ToList());
            }

            return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                false, default!, default!);
        }

        public override async Task<KeyValueMetadataFetchResponse<byte[], Metadata>> FetchMetadataAndValueAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata",
                keyValueMetadataFetcherClient is null,
                cancellationToken);

            var request = new FetchMetadataAndValueRequest()
            {
                Key = key,
            };

            var response = await keyValueMetadataFetcherClient!.FetchMetadataAndValueAsync(
                request, cancellationToken: cancellationToken);

            if (response.Exists)
            {
                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    true,
                    response.Value.ToByteArray(),
                    response.Metadata.ToList());
            }

            return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                false, default!, default!);
        }

        #endregion

        #region [ IKeyValueStorer ]

        public override async Task StoreAsync(
            string key,
            byte[] value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store,
                "Store",
                keyValueStorerClient is null,
                cancellationToken);

            var request = new StoreRequest()
            {
                Key = key,
                Value = ByteString.CopyFrom(value),
                StoreMode = (StoreMode)storeMode,
            };

            await keyValueStorerClient!.StoreAsync(
                request, cancellationToken: cancellationToken);
        }

        public override async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store,
                "Store",
                keyValueStorerClient is null,
                cancellationToken);

            var request = new RemoveRequest()
            {
                Key = key,
            };

            await keyValueStorerClient!.RemoveAsync(
                request, cancellationToken: cancellationToken);
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public override async Task StoreMetadataAsync(
            string key,
            Metadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata",
                keyValueMetadataStorerClient is null,
                cancellationToken);

            var request = new StoreMetadataRequest()
            {
                Key = key,
                StoreMode = (StoreMode)storeMode,
            };

            foreach (var pair in metadata)
            {
                request.Metadata.Add(pair.Key, pair.Value);
            }

            await keyValueMetadataStorerClient!.StoreMetadataAsync(
                request, cancellationToken: cancellationToken);
        }

        public override async Task StoreMetadataAndValueAsync(
            string key,
            byte[] value,
            Metadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata",
                keyValueMetadataStorerClient is null,
                cancellationToken);

            var request = new StoreMetadataAndValueRequest()
            {
                Key = key,
                Value = ByteString.CopyFrom(value),
                StoreMode = (StoreMode)storeMode,
            };

            foreach (var pair in metadata)
            {
                request.Metadata.Add(pair.Key, pair.Value);
            }

            await keyValueMetadataStorerClient!.StoreMetadataAndValueAsync(
                request, cancellationToken: cancellationToken);
        }

        #endregion

        #region [ IKeyLister ]

        public override async Task<ICollection<KeyListerItem<string>>> ListKeysAsync(
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List,
                "List",
                keyListerClient is null,
                cancellationToken);

            var request = new ListKeysRequest();

            var response = await keyListerClient!.ListKeysAsync(
                request, cancellationToken: cancellationToken);

            return ToKeyListerItems(response.Items);
        }

        #endregion

        #region [ IKeyMetadataLister ]

        public override async Task<ICollection<KeyMetadataListerItem<string, Metadata>>> ListMetadataKeysAsync(
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.Metadata,
                "List and Metadata",
                keyMetadataListerClient is null,
                cancellationToken);

            var request = new ListMetadataKeysRequest();

            var response = await keyMetadataListerClient!.ListMetadataKeysAsync(
                request, cancellationToken: cancellationToken);

            return ToKeyMetadataListerItems(response.Items);
        }

        #endregion

        #region [ IKeyAsyncLister ]

        public override async IAsyncEnumerable<ICollection<KeyListerItem<string>>> ListAsyncKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList,
                "Async List",
                keyAsyncListerClient is null,
                cancellationToken);

            var request = new ListAsyncKeysRequest();

            var response = keyAsyncListerClient!.ListAsyncKeys(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyListerItems(
                response.ResponseStream,
                page => page.Items,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyAsyncMetadataLister ]

        public override async IAsyncEnumerable<ICollection<KeyMetadataListerItem<string, Metadata>>> ListAsyncMetadataKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.Metadata,
                "Async List and Metadata",
                keyAsyncMetadataListerClient is null,
                cancellationToken);

            var request = new ListAsyncMetadataKeysRequest();

            var response = keyAsyncMetadataListerClient!.ListAsyncMetadataKeys(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyMetadataListerItems(
                response.ResponseStream,
                page => page.Items,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyPrefixLister ]

        public override async Task<ICollection<KeyListerItem<string>>> ListPrefixedKeysAsync(
            string keyPrefix,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.KeyPrefix,
                "List and KeyPrefix",
                keyPrefixListerClient is null,
                cancellationToken);

            var request = new ListPrefixedKeysRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = await keyPrefixListerClient!.ListPrefixedKeysAsync(
                request, cancellationToken: cancellationToken);

            return ToKeyListerItems(response.Items);
        }

        #endregion

        #region [ IKeyPrefixMetadataLister ]

        public override async Task<ICollection<KeyMetadataListerItem<string, Metadata>>> ListPrefixedMetadataKeysAsync(
            string keyPrefix,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.KeyPrefix | KeyValueStorageCapability.Metadata,
                "List, KeyPrefix and Metadata",
                keyPrefixMetadataListerClient is null,
                cancellationToken);

            var request = new ListPrefixedMetadataKeysRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = await keyPrefixMetadataListerClient!.ListPrefixedMetadataKeysAsync(
                request, cancellationToken: cancellationToken);

            return ToKeyMetadataListerItems(response.Items);
        }

        #endregion

        #region [ IKeyPrefixAsyncLister ]

        public override async IAsyncEnumerable<ICollection<KeyListerItem<string>>> ListAsyncPrefixedKeys(
            string keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.KeyPrefix,
                "Async List and KeyPrefix",
                keyPrefixAsyncListerClient is null,
                cancellationToken);

            var request = new ListAsyncPrefixedKeysRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = keyPrefixAsyncListerClient!.ListAsyncPrefixedKeys(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyListerItems(
                response.ResponseStream,
                page => page.Items,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyPrefixAsyncMetadataLister ]

        public override async IAsyncEnumerable<ICollection<KeyMetadataListerItem<string, Metadata>>> ListAsyncPrefixedMetadataKeys(
            string keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.Metadata | KeyValueStorageCapability.KeyPrefix,
                "Async List, Metadata and KeyPrefix",
                keyPrefixAsyncMetadataListerClient is null,
                cancellationToken);

            var request = new ListAsyncPrefixedMetadataKeysRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = keyPrefixAsyncMetadataListerClient!.ListAsyncPrefixedMetadataKeys(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyMetadataListerItems(
                response.ResponseStream,
                page => page.Items,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyStoreEvents ]

        public override async IAsyncEnumerable<ICollection<KeyStoreEvent<string>>> SubscribeKeyEvents(
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents,
                "StoreEvents",
                keyStoreEventsClient is null,
                cancellationToken);

            var request = new SubscribeKeyEventsRequest();

            var response = keyStoreEventsClient!.SubscribeKeyEvents(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyStoreEvents(
                response.ResponseStream,
                page => page.Events,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyMetadataStoreEvents ]

        public override async IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<string, Metadata>>> SubscribeKeyMetadataEvents(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.Metadata,
                "StoreEvents and Metadata",
                keyMetadataStoreEventsClient is null,
                cancellationToken);

            var request = new SubscribeKeyMetadataEventsRequest();

            var response = keyMetadataStoreEventsClient!.SubscribeKeyMetadataEvents(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyMetadataStoreEvents(
                response.ResponseStream,
                page => page.Events,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyPrefixStoreEvents ]

        public override async IAsyncEnumerable<ICollection<KeyStoreEvent<string>>> SubscribeKeyPrefixEvents(
            string keyPrefix, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.KeyPrefix,
                "StoreEvents and KeyPrefix",
                keyPrefixStoreEventsClient is null,
                cancellationToken);

            var request = new SubscribeKeyPrefixEventsRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = keyPrefixStoreEventsClient!.SubscribeKeyPrefixEvents(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyStoreEvents(
                response.ResponseStream,
                page => page.Events,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ IKeyPrefixMetadataStoreEvents ]

        public override async IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<string, Metadata>>> SubscribeKeyPrefixMetadataEvents(
            string keyPrefix, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.Metadata | KeyValueStorageCapability.KeyPrefix,
                "StoreEvents, Metadata and KeyPrefix",
                keyPrefixMetadataStoreEventsClient is null,
                cancellationToken);

            var request = new SubscribeKeyPrefixMetadataEventsRequest()
            {
                KeyPrefix = keyPrefix,
            };

            var response = keyPrefixMetadataStoreEventsClient!.SubscribeKeyPrefixMetadataEvents(
                request, cancellationToken: cancellationToken);

            await foreach (var item in ToKeyMetadataStoreEvents(
                response.ResponseStream,
                page => page.Events,
                cancellationToken))
            {
                yield return item;
            }
        }

        #endregion

        #region [ Selectors ]

        private static KeyListerItem<string> ToKeyListerItem(KeyListerItem item) =>
            new(item.Key);

        private static KeyMetadataListerItem<string, Metadata> ToKeyMetadataListerItem(
            KeyMetadataListerItem item) => new(item.Key, item.Metadata.ToList());

        private static KeyStoreEvent<string> ToKeyStoreEvent(KeyStoreEvent @event) =>
            @event.Type switch
            {
                KeyStoreEventType.Created =>
                    new KeyStoreEvent<string>.Created(@event.Key),

                KeyStoreEventType.Replaced =>
                    new KeyStoreEvent<string>.Replaced(@event.Key),

                KeyStoreEventType.Removed =>
                    new KeyStoreEvent<string>.Removed(@event.Key),

                _ => throw new NotSupportedException(),
            };

        private static KeyMetadataStoreEvent<string, Metadata> ToKeyMetadataStoreEvent(KeyMetadataStoreEvent @event) =>
            @event.Type switch
            {
                KeyStoreEventType.Created =>
                    new KeyMetadataStoreEvent<string, Metadata>.Created(
                        @event.Key, @event.Metadata.ToList()),

                KeyStoreEventType.Replaced =>
                    new KeyMetadataStoreEvent<string, Metadata>.Replaced(
                        @event.Key, @event.Metadata.ToList(), 
                        @event.ReplacedMetadata, @event.ReplacedValue),

                KeyStoreEventType.Removed =>
                    new KeyMetadataStoreEvent<string, Metadata>.Removed(
                        @event.Key, @event.Metadata.ToList()),

                _ => throw new NotSupportedException(),
            };

        private static ICollection<KeyListerItem<string>> ToKeyListerItems(IEnumerable<KeyListerItem> source) =>
            source.Select(ToKeyListerItem).ToList();

        private static IAsyncEnumerable<ICollection<KeyListerItem<string>>> ToKeyListerItems<TPage>(
            IAsyncStreamReader<TPage> source,
            Func<TPage, IEnumerable<KeyListerItem>> toItems,
            CancellationToken cancellationToken) =>
            ToAsyncEnumerable(source, cancellationToken)
                .Select(toItems)
                .Select(ToKeyListerItems);

        private static ICollection<KeyMetadataListerItem<string, Metadata>> ToKeyMetadataListerItems(
            IEnumerable<KeyMetadataListerItem> source) =>
            source.Select(ToKeyMetadataListerItem).ToList();

        private static IAsyncEnumerable<ICollection<KeyMetadataListerItem<string, Metadata>>> ToKeyMetadataListerItems<TPage>(
            IAsyncStreamReader<TPage> source,
            Func<TPage, IEnumerable<KeyMetadataListerItem>> toItems,
            CancellationToken cancellationToken) =>
            ToAsyncEnumerable(source, cancellationToken)
                .Select(toItems)
                .Select(ToKeyMetadataListerItems);

        private static ICollection<KeyStoreEvent<string>> ToKeyStoreEvents(IEnumerable<KeyStoreEvent> source) =>
            source.Select(ToKeyStoreEvent).ToList();

        private static IAsyncEnumerable<ICollection<KeyStoreEvent<string>>> ToKeyStoreEvents<TPage>(
            IAsyncStreamReader<TPage> source,
            Func<TPage, IEnumerable<KeyStoreEvent>> toItems,
            CancellationToken cancellationToken) =>
            ToAsyncEnumerable(source, cancellationToken)
                .Select(toItems)
                .Select(ToKeyStoreEvents);

        private static ICollection<KeyMetadataStoreEvent<string, Metadata>> ToKeyMetadataStoreEvents(
            IEnumerable<KeyMetadataStoreEvent> source) =>
            source.Select(ToKeyMetadataStoreEvent).ToList();

        private static IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<string, Metadata>>> ToKeyMetadataStoreEvents<TPage>(
            IAsyncStreamReader<TPage> source,
            Func<TPage, IEnumerable<KeyMetadataStoreEvent>> toItems,
            CancellationToken cancellationToken) =>
            ToAsyncEnumerable(source, cancellationToken)
                .Select(toItems)
                .Select(ToKeyMetadataStoreEvents);

        private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(
            IAsyncStreamReader<T> source,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {

            while (await source.MoveNext(cancellationToken))
            {
                yield return source.Current;
            }
        }

        #endregion
    }
}
