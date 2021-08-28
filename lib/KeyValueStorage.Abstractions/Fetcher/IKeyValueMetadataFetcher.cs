using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public record KeyValueMetadataFetchResponse(
        bool Exists,
        object Value,
        object Metadata);

    public record KeyValueMetadataFetchResponse<TValue, TMetadata>(
        bool Exists,
        TValue Value,
        TMetadata Metadata);

    public interface IKeyValueMetadataFetcher :
        IKeyValueFetcher
    {
        Task<KeyValueMetadataFetchResponse> FetchMetadataAsync(
            object key,
            CancellationToken cancellationToken = default);

        Task<KeyValueMetadataFetchResponse> FetchMetadataAndValueAsync(
            object key,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyValueMetadataFetcher<TKey, TValue, TMetadata> :
        IKeyValueFetcher<TKey, TValue>,
        IKeyValueMetadataFetcher
    {
        Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAsync(
            TKey key,
            CancellationToken cancellationToken = default);

        Task<KeyValueMetadataFetchResponse<TValue, TMetadata>> FetchMetadataAndValueAsync(
            TKey key,
            CancellationToken cancellationToken = default);
    }
}
