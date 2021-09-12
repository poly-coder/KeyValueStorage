using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyPrefixMetadataLister :
        IKeyPrefixLister
    {
        Task<ICollection<KeyMetadataListerItem>> ListPrefixedMetadataKeysAsync(
            object keyPrefix,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyPrefixMetadataLister<TKey, TMetadata> :
        IKeyPrefixMetadataLister,
        IKeyPrefixLister<TKey>
    {
        Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListPrefixedMetadataKeysAsync(
            TKey keyPrefix,
            CancellationToken cancellationToken = default);
    }
}