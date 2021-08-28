using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public abstract class LocalKeyValueStorageBase<TKey, TValue, TMetadata> :
        IKeyValueStorage,
        IKeyValueMetadataFetcher<TKey, TValue, TMetadata>,
        IKeyValueMetadataStorer<TKey, TValue, TMetadata>,
        IKeyAsyncMetadataLister<TKey, TMetadata>
    {
        private readonly KeyValueStorageCapability capabilities;

        protected LocalKeyValueStorageBase(
            KeyValueStorageCapability capabilities)
        {
            this.capabilities = capabilities;
        }

        [DoesNotReturn]
        private void CheckCapability(KeyValueStorageCapability capability, string name)
        {
            if (capabilities.HasFlag(capability))
            {
                throw new NotImplementedException($"Capability {capability} is not implemented, even when declared");
            }

            throw new NotSupportedException($"Capability {capability} is not supported");
        }

        #region [ IKeyValueStorage ]

        public Task<KeyValueStorageCapability> GetCapabilitiesAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(capabilities);
        }

        #endregion

        #region [ IKeyValueMetadataFetcher ]

        public virtual async Task<KeyValueFetchResponse<TValue>> FetchAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(KeyValueStorageCapability.Fetch, "Fetch");

            return null;
        }

        public virtual async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata");

            return null;
        }

        public virtual async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAndValueAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata");

            return null;
        }

        // Explicit

        async Task<KeyValueFetchResponse> IKeyValueFetcher.FetchAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchAsync(
                (TKey)key,
                cancellationToken);

            return new KeyValueFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object));
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAsync((TKey)key, cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object),
                default);
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAndValueAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAndValueAsync((TKey)key, cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                response.Exists ? response.Value : default(object),
                response.Exists ? response.Metadata : default(object));
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public virtual async Task StoreAsync(
            TKey key,
            TValue value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Store,
                "Store");
        }

        public virtual async Task RemoveAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Store,
                "Store");
        }

        public virtual async Task StoreMetadataAsync(
            TKey key,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata");
        }

        public virtual async Task StoreMetadataAndValueAsync(
            TKey key,
            TValue value,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata");
        }

        // Explicit

        async Task IKeyValueStorer.StoreAsync(
            object key,
            object value,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreAsync((TKey)key, (TValue)value, storeMode, cancellationToken);
        }

        async Task IKeyValueStorer.RemoveAsync(
            object key,
            CancellationToken cancellationToken)
        {
            await RemoveAsync((TKey)key, cancellationToken);
        }

        async Task IKeyValueMetadataStorer.StoreMetadataAsync(
            object key,
            object metadata,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreMetadataAsync((TKey)key, (TMetadata)metadata, storeMode, cancellationToken);
        }

        async Task IKeyValueMetadataStorer.StoreMetadataAndValueAsync(
            object key,
            object value,
            object metadata,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreMetadataAndValueAsync((TKey)key, (TValue)value, (TMetadata)metadata, storeMode, cancellationToken);
        }

        #endregion

        #region [ IKeyAsyncMetadataLister ]

        public virtual async Task<ICollection<KeyListerItem<TKey>>> ListKeysAsync(
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.List,
                "List");

            return null;
        }

        public virtual async Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListMetadataKeysAsync(
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.Metadata,
                "List and Metadata");

            return null;
        }

        public virtual IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> ListAsyncKeys(
            CancellationToken cancellationToken)
        {
            CheckCapability(
                KeyValueStorageCapability.AsyncList,
                "AsyncList");

            return null;
        }

        public virtual IAsyncEnumerable<ICollection<KeyMetadataListerItem>> ListAsyncMetadataKeys(
            CancellationToken cancellationToken)
        {
            CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.Metadata,
                "AsyncList and Metadata");

            return null;
        }

        // Explicit

        async Task<ICollection<KeyListerItem>> IKeyLister.ListKeysAsync(
            CancellationToken cancellationToken)
        {
            var result = await ListMetadataKeysAsync(cancellationToken);

            return result
                .Select(e => new KeyListerItem(e.Key))
                .ToList();
        }

        async Task<ICollection<KeyMetadataListerItem>> IKeyMetadataLister.ListMetadataKeysAsync(
            CancellationToken cancellationToken)
        {
            var result = await ListMetadataKeysAsync(cancellationToken);

            return result
                .Select(e => new KeyMetadataListerItem(e.Key, e.Metadata))
                .ToList();
        }

        async IAsyncEnumerable<ICollection<KeyListerItem>> IKeyAsyncLister.ListAsyncKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var items in ListAsyncKeys(cancellationToken))
            {
                yield return items
                    .Select(e => new KeyListerItem(e.Key))
                    .ToList();
            }
        }

        async IAsyncEnumerable<ICollection<KeyMetadataListerItem>> IKeyAsyncMetadataLister.ListAsyncMetadataKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var items in ListAsyncMetadataKeys(cancellationToken))
            {
                yield return items
                    .Select(e => new KeyMetadataListerItem(e.Key, e.Metadata))
                    .ToList();
            }
        }

        #endregion
    }
}