using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public record KeyMetadataListerItem(
        object Key, object Metadata);

    public record KeyMetadataListerItem<TKey, TMetadata>(
        TKey Key, TMetadata Metadata);

    public interface IKeyMetadataLister :
        IKeyLister
    {
        Task<ICollection<KeyMetadataListerItem>> ListMetadataKeysAsync(
            CancellationToken cancellationToken = default);
    }

    public interface IKeyMetadataLister<TKey, TMetadata> :
        IKeyLister<TKey>,
        IKeyMetadataLister
    {
        new Task<ICollection<KeyMetadataListerItem<TKey, TMetadata>>> ListMetadataKeysAsync(
            CancellationToken cancellationToken = default);
    }
}