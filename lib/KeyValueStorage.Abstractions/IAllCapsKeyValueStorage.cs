namespace KeyValueStorage.Abstractions
{
    public interface IAllCapsKeyValueStorage :
        IKeyValueStorage,
        IKeyValueFetcher,
        IKeyValueMetadataFetcher,
        IKeyValueStorer,
        IKeyValueMetadataStorer,
        IKeyLister,
        IKeyMetadataLister,
        IKeyAsyncLister,
        IKeyAsyncMetadataLister,
        IKeyPrefixLister,
        IKeyPrefixMetadataLister,
        IKeyPrefixAsyncLister,
        IKeyPrefixAsyncMetadataLister,
        IKeyStoreEvents,
        IKeyMetadataStoreEvents,
        IKeyPrefixStoreEvents,
        IKeyPrefixMetadataStoreEvents
    {

    }

    public interface IAllCapsKeyValueStorage<TKey, TValue, TMetadata> :
        IAllCapsKeyValueStorage,
        IKeyValueFetcher<TKey, TValue>,
        IKeyValueMetadataFetcher<TKey, TValue, TMetadata>,
        IKeyValueStorer<TKey, TValue>,
        IKeyValueMetadataStorer<TKey, TValue, TMetadata>,
        IKeyLister<TKey>,
        IKeyMetadataLister<TKey, TMetadata>,
        IKeyAsyncLister<TKey>,
        IKeyAsyncMetadataLister<TKey, TMetadata>,
        IKeyPrefixLister<TKey>,
        IKeyPrefixMetadataLister<TKey, TMetadata>,
        IKeyPrefixAsyncLister<TKey>,
        IKeyPrefixAsyncMetadataLister<TKey, TMetadata>,
        IKeyStoreEvents<TKey>,
        IKeyMetadataStoreEvents<TKey, TMetadata>,
        IKeyPrefixStoreEvents<TKey>,
        IKeyPrefixMetadataStoreEvents<TKey, TMetadata>
    {

    }
}
