using DotNetX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public abstract class KeyValueStorageBase<TKey, TValue, TMetadata> :
        IAllCapsKeyValueStorage<TKey, TValue, TMetadata>
    {
        protected KeyValueStorageBase()
        {
            capabilitiesCache = new DelegateReliableAsyncService<KeyValueStorageCapability>(GetCapabilitiesOverride);
        }

        #region [ IKeyValueStorage ]

        private readonly IReliableAsyncService<KeyValueStorageCapability> capabilitiesCache;

        public async Task<KeyValueStorageCapability> GetCapabilitiesAsync(
            CancellationToken cancellationToken = default)
        {
            return await capabilitiesCache.GetServiceAsync(cancellationToken);
        }

        protected abstract Task<KeyValueStorageCapability> GetCapabilitiesOverride(
            CancellationToken cancellationToken);

        #endregion

        #region [ IKeyValueFetcher ]

        public virtual async Task<KeyValueFetchResponse<TValue>> FetchAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch,
                "Fetch",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<KeyValueFetchResponse> IKeyValueFetcher.FetchAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchAsync(
                (TKey)key,
                cancellationToken);

            return new KeyValueFetchResponse(
                response.Exists,
                response.Value!);
        }

        #endregion

        #region [ IKeyValueMetadataFetcher ]

        public virtual async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata",
                cancellationToken);


            throw new NotSupportedException();
        }

        public virtual async Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAndValueAsync(
            TKey key,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Fetch | KeyValueStorageCapability.Metadata,
                "Fetch and Metadata",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAsync(
                (TKey)key,
                cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                default!,
                response.Metadata!);
        }

        async Task<KeyValueMetadataFetchResponse> IKeyValueMetadataFetcher.FetchMetadataAndValueAsync(
            object key,
            CancellationToken cancellationToken)
        {
            var response = await FetchMetadataAndValueAsync(
                (TKey)key,
                cancellationToken);

            return new KeyValueMetadataFetchResponse(
                response.Exists,
                response.Value!,
                response.Metadata!);
        }

        #endregion

        #region [ IKeyValueStorer ]

        public virtual async Task StoreAsync(
            TKey key, 
            TValue value, 
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store,
                "Store",
                cancellationToken);
        }

        public virtual async Task RemoveAsync(
            TKey key, 
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store,
                "Store",
                cancellationToken);
        }

        async Task IKeyValueStorer.StoreAsync(object key, object value, KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreAsync(
                (TKey)key,
                (TValue)value,
                storeMode,
                cancellationToken);
        }

        async Task IKeyValueStorer.RemoveAsync(object key, CancellationToken cancellationToken)
        {
            await RemoveAsync(
                (TKey)key,
                cancellationToken);
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public virtual async Task StoreMetadataAsync(
            TKey key,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata",
                cancellationToken);
        }

        public virtual async Task StoreMetadataAndValueAsync(
            TKey key,
            TValue value,
            TMetadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.Store | KeyValueStorageCapability.Metadata,
                "Store and Metadata",
                cancellationToken);
        }

        async Task IKeyValueMetadataStorer.StoreMetadataAsync(
            object key,
            object metadata,
            KeyValueStoreMode storeMode,
            CancellationToken cancellationToken)
        {
            await StoreMetadataAsync(
                (TKey)key,
                (TMetadata)metadata,
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
                (TKey)key,
                (TValue)value,
                (TMetadata)metadata,
                storeMode,
                cancellationToken);
        }

        #endregion

        #region [ IKeyLister ]

        public virtual async Task<ICollection<KeyListerItem<TKey>>> ListKeysAsync(
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List,
                "List",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<ICollection<KeyListerItem>> IKeyLister.ListKeysAsync(
            CancellationToken cancellationToken) =>
            ToKeyListerItems(
                await ListKeysAsync(
                    cancellationToken));

        #endregion

        #region [ IKeyMetadataLister ]

        public virtual async Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListMetadataKeysAsync(
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.Metadata,
                "List and Metadata",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<ICollection<KeyMetadataListerItem>> IKeyMetadataLister.ListMetadataKeysAsync(
            CancellationToken cancellationToken) =>
            ToKeyMetadataListerItems(
                await ListMetadataKeysAsync(
                    cancellationToken));

        #endregion

        #region [ IKeyAsyncLister ]

        public virtual async IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> ListAsyncKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList,
                "AsyncList",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyListerItem>> IKeyAsyncLister.ListAsyncKeys(
            CancellationToken cancellationToken) =>
            ToKeyListerItems(ListAsyncKeys(cancellationToken));

        #endregion

        #region [ IKeyAsyncMetadataLister ]

        public virtual async IAsyncEnumerable<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListAsyncMetadataKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.Metadata,
                "AsyncList and Metadata",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyMetadataListerItem>> IKeyAsyncMetadataLister.ListAsyncMetadataKeys(
            CancellationToken cancellationToken) =>
            ToKeyMetadataListerItems(ListAsyncMetadataKeys(cancellationToken));

        #endregion

        #region [ IKeyPrefixLister ]

        public virtual async Task<ICollection<KeyListerItem<TKey>>> ListPrefixedKeysAsync(
            TKey keyPrefix,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.KeyPrefix,
                "List and KeyPrefix",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<ICollection<KeyListerItem>> IKeyPrefixLister.ListPrefixedKeysAsync(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyListerItems(await ListPrefixedKeysAsync((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ IKeyPrefixMetadataLister ]

        public virtual async Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListPrefixedMetadataKeysAsync(
            TKey keyPrefix,
            CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.List | KeyValueStorageCapability.Metadata | KeyValueStorageCapability.KeyPrefix,
                "List, Metadata and KeyPrefix",
                cancellationToken);

            throw new NotSupportedException();
        }

        async Task<ICollection<KeyMetadataListerItem>> IKeyPrefixMetadataLister.ListPrefixedMetadataKeysAsync(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyMetadataListerItems(await ListPrefixedMetadataKeysAsync((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ IKeyPrefixAsyncLister ]

        public virtual async IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> ListAsyncPrefixedKeys(
            TKey keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.KeyPrefix,
                "AsyncList and KeyPrefix",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyListerItem>> IKeyPrefixAsyncLister.ListAsyncPrefixedKeys(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyListerItems(ListAsyncPrefixedKeys((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ IKeyPrefixAsyncMetadataLister ]

        public virtual async IAsyncEnumerable<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListAsyncPrefixedMetadataKeys(
            TKey keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.AsyncList | KeyValueStorageCapability.Metadata | KeyValueStorageCapability.KeyPrefix,
                "AsyncList, Metadata and KeyPrefix",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyMetadataListerItem>> IKeyPrefixAsyncMetadataLister.ListAsyncPrefixedMetadataKeys(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyMetadataListerItems(ListAsyncPrefixedMetadataKeys((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ IKeyStoreEvents ]

        public virtual async IAsyncEnumerable<ICollection<KeyStoreEvent<TKey>>> SubscribeKeyEvents(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents,
                "StoreEvents",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyStoreEvent>> IKeyStoreEvents.SubscribeKeyEvents(
            CancellationToken cancellationToken) =>
            ToKeyStoreEvents(SubscribeKeyEvents(cancellationToken));


        #endregion

        #region [ IKeyMetadataStoreEvents ]

        public virtual async IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<TKey, TMetadata>>> SubscribeKeyMetadataEvents(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.Metadata,
                "StoreEvents and Metadata",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyMetadataStoreEvent>> IKeyMetadataStoreEvents.SubscribeKeyMetadataEvents(
            CancellationToken cancellationToken) =>
            ToKeyMetadataStoreEvents(SubscribeKeyMetadataEvents(cancellationToken));

        #endregion

        #region [ IKeyPrefixStoreEvents ]

        public virtual async IAsyncEnumerable<ICollection<KeyStoreEvent<TKey>>> SubscribeKeyPrefixEvents(
            TKey keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.KeyPrefix,
                "StoreEvents and KeyPrefix",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyStoreEvent>> IKeyPrefixStoreEvents.SubscribeKeyPrefixEvents(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyStoreEvents(SubscribeKeyPrefixEvents((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ IKeyPrefixMetadataStoreEvents ]

        public virtual async IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<TKey, TMetadata>>> SubscribeKeyPrefixMetadataEvents(
            TKey keyPrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await CheckCapability(
                KeyValueStorageCapability.StoreEvents | KeyValueStorageCapability.Metadata | KeyValueStorageCapability.KeyPrefix,
                "StoreEvents, Metadata and KeyPrefix",
                cancellationToken);

            yield break;
        }

        IAsyncEnumerable<ICollection<KeyMetadataStoreEvent>> IKeyPrefixMetadataStoreEvents.SubscribeKeyPrefixMetadataEvents(
            object keyPrefix,
            CancellationToken cancellationToken) =>
            ToKeyMetadataStoreEvents(SubscribeKeyPrefixMetadataEvents((TKey)keyPrefix, cancellationToken));

        #endregion

        #region [ Internals ]

        private async Task CheckCapability(
            KeyValueStorageCapability capability,
            string name,
            CancellationToken cancellationToken)
        {
            var capabilities = await GetCapabilitiesAsync(cancellationToken);

            if (!capabilities.HasFlag(capability))
            {
                throw new NotImplementedException($"Capability {name} is not implemented, even when declared");
            }

            throw new NotSupportedException($"Capability {name} is not supported");
        }

        #endregion

        #region [ Selectors ]

        private static KeyListerItem ToKeyListerItem(
            KeyListerItem<TKey> item) =>
            new(item.Key!);

        private static KeyMetadataListerItem ToKeyMetadataListerItem(
            KeyMetadataListerItem<TKey, TMetadata> item) =>
            new(item.Key!, item.Metadata!);

        private static KeyStoreEvent ToKeyStoreEvent(
            KeyStoreEvent<TKey> @event) =>
            @event switch
            {
                KeyStoreEvent<TKey>.Created(var key) => new KeyStoreEvent.Created(key!),
                KeyStoreEvent<TKey>.Replaced(var key) => new KeyStoreEvent.Replaced(key!),
                KeyStoreEvent<TKey>.Removed(var key) => new KeyStoreEvent.Removed(key!),
                _ => throw new NotSupportedException(),
            };

        private static KeyMetadataStoreEvent ToKeyMetadataStoreEvent(
            KeyMetadataStoreEvent<TKey, TMetadata> @event) =>
            @event switch
            {
                KeyMetadataStoreEvent<TKey, TMetadata>.Created(var key, var metadata) =>
                    new KeyMetadataStoreEvent.Created(key!, metadata!),
                KeyMetadataStoreEvent<TKey, TMetadata>.Replaced(var key, var metadata, var replacedMetadata, var replacedValue) =>
                    new KeyMetadataStoreEvent.Replaced(key!, metadata!, replacedMetadata, replacedValue),
                KeyMetadataStoreEvent<TKey, TMetadata>.Removed(var key, var metadata) =>
                    new KeyMetadataStoreEvent.Removed(key!, metadata!),
                _ => throw new NotSupportedException(),
            };
        //
        // private static KeyMetadataListerItem ToKeyMetadataListerItem(
        //     KeyMetadataListerItem<TKey, TMetadata> e) =>
        //     new(e.Key!, e.Metadata!);

        private static ICollection<KeyListerItem> ToKeyListerItems(
            IEnumerable<KeyListerItem<TKey>> source) =>
            source.Select(ToKeyListerItem).ToList();

        private static IAsyncEnumerable<ICollection<KeyListerItem>> ToKeyListerItems(
            IAsyncEnumerable<ICollection<KeyListerItem<TKey>>> source) =>
            source.Select(ToKeyListerItems);

        private static ICollection<KeyMetadataListerItem> ToKeyMetadataListerItems(
            IEnumerable<KeyMetadataListerItem<TKey, TMetadata>> source) =>
            source.Select(ToKeyMetadataListerItem).ToList();

        private static IAsyncEnumerable<ICollection<KeyMetadataListerItem>> ToKeyMetadataListerItems(
            IAsyncEnumerable<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> source) =>
            source.Select(ToKeyMetadataListerItems);

        private static ICollection<KeyStoreEvent> ToKeyStoreEvents(
            IEnumerable<KeyStoreEvent<TKey>> source) =>
            source.Select(ToKeyStoreEvent).ToList();

        private static IAsyncEnumerable<ICollection<KeyStoreEvent>> ToKeyStoreEvents(
            IAsyncEnumerable<ICollection<KeyStoreEvent<TKey>>> source) =>
            source.Select(ToKeyStoreEvents);

        private static ICollection<KeyMetadataStoreEvent> ToKeyMetadataStoreEvents(
            IEnumerable<KeyMetadataStoreEvent<TKey, TMetadata>> source) =>
            source.Select(ToKeyMetadataStoreEvent).ToList();

        private static IAsyncEnumerable<ICollection<KeyMetadataStoreEvent>> ToKeyMetadataStoreEvents(
            IAsyncEnumerable<ICollection<KeyMetadataStoreEvent<TKey, TMetadata>>> source) =>
            source.Select(ToKeyMetadataStoreEvents);

        #endregion
    }
}