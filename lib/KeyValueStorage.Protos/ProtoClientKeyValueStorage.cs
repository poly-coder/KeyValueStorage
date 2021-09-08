using KeyValueStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace KeyValueStorage.Protos
{
    // TODO: Catch RpcException to handle errors
    // TODO: Pass a Credentials provider in case auth is required
    // TODO: Pass OpenTelemetry headers and create StartActivity when a call is received with corresponding headers
    public class ProtoClientKeyValueStorage :
        IKeyValueStorage,
        IKeyValueFetcher<string, byte[]>,
        IKeyValueMetadataFetcher<string, byte[], IEnumerable<KeyValuePair<string, string>>>,
        IKeyValueStorer<string, byte[]>,
        IKeyValueMetadataStorer<string, byte[], IEnumerable<KeyValuePair<string, string>>>
    // IKeyAsyncMetadataLister<string, IEnumerable<KeyValuePair<string, string>>>,
    // IKeyPrefixAsyncMetadataLister<string, IEnumerable<KeyValuePair<string, string>>>

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
            KeyPrefixAsyncMetadataLister.KeyPrefixAsyncMetadataListerClient? keyPrefixAsyncMetadataListerClient = null
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
        }


        private async Task CheckCapability(
            KeyValueStorageCapability capability,
            string name,
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

        #region [ IKeyValueStorage ]

        private KeyValueStorageCapability? _capabilities;

        public async Task<KeyValueStorageCapability> GetCapabilitiesAsync(
            CancellationToken cancellationToken = default)
        {
            if (_capabilities is null)
            {
                _capabilities = await GetRemoteCapabilitiesAsync(cancellationToken);
            }

            return _capabilities.Value;

        }

        private async Task<KeyValueStorageCapability> GetRemoteCapabilitiesAsync(
            CancellationToken cancellationToken)
        {
            var request = new GetCapabilitiesRequest();

            var response = await keyValueStorageClient.GetCapabilitiesAsync(
                request, cancellationToken: cancellationToken);

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

        #endregion

        #region [ IKeyValueFetcher ]

        public async Task<KeyValueFetchResponse<byte[]>> FetchAsync(
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

        async Task<KeyValueFetchResponse> IKeyValueFetcher.FetchAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchAsync(
                (string)key,
                cancellationToken);

            return new KeyValueFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object));
        }

        #endregion

        #region [ IKeyValueMetadataFetcher ]

        public async Task<KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>> FetchMetadataAsync(
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
                return new KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>(
                    true,
                    default!,
                    response.Metadata.ToList());
            }

            return new KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>(
                false, default!, default!);
        }

        public async Task<KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>> FetchMetadataAndValueAsync(
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
                return new KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>(
                    true,
                    response.Value.ToByteArray(),
                    response.Metadata.ToList());
            }

            return new KeyValueMetadataFetchResponse<byte[], IEnumerable<KeyValuePair<string, string>>>(
                false, default!, default!);
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAsync(
            object key, 
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAsync(
                (string)key,
                cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object),
                default);
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAndValueAsync(
            object key, 
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAndValueAsync(
                (string)key,
                cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object),
                response.Exists ? response.Metadata : default(object));
        }

        #endregion

        #region [ IKeyValueStorer ]

        public async Task StoreAsync(
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

        public async Task RemoveAsync(
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

        async Task IKeyValueStorer.StoreAsync(
            object key,
            object value,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreAsync(
                (string)key,
                (byte[])value,
                storeMode,
                cancellationToken);
        }

        async Task IKeyValueStorer.RemoveAsync(
            object key,
            CancellationToken cancellationToken)
        {
            await RemoveAsync(
                (string)key,
                cancellationToken);
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public async Task StoreMetadataAsync(
            string key,
            IEnumerable<KeyValuePair<string, string>> metadata,
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

        public async Task StoreMetadataAndValueAsync(
            string key,
            byte[] value,
            IEnumerable<KeyValuePair<string, string>> metadata,
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

        async Task IKeyValueMetadataStorer.StoreMetadataAsync(
            object key,
            object metadata,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreMetadataAsync(
                (string)key,
                (IEnumerable<KeyValuePair<string, string>>)metadata,
                storeMode,
                cancellationToken);
        }

        async Task IKeyValueMetadataStorer.StoreMetadataAndValueAsync(
            object key,
            object value,
            object metadata,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreMetadataAndValueAsync(
                (string)key,
                (byte[])value,
                (IEnumerable<KeyValuePair<string, string>>)metadata,
                storeMode,
                cancellationToken);
        }

        #endregion
    }
}
