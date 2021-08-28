using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public record KeyValueFetchResponse(
        bool Exists,
        object Value);

    public record KeyValueFetchResponse<TValue>(
        bool Exists,
        TValue Value);

    public interface IKeyValueFetcher :
        IKeyValueStorage
    {
        Task<KeyValueFetchResponse> FetchAsync(
            object key,
            CancellationToken cancellationToken = default);
    }

    public interface IKeyValueFetcher<TKey, TValue> :
        IKeyValueFetcher
    {
        Task<KeyValueFetchResponse<TValue>> FetchAsync(TKey key, CancellationToken cancellationToken = default);
    }
}