using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyValueStorage
    {
        // TODO: add prefix separator
        Task<KeyValueStorageCapability> GetCapabilitiesAsync(
            CancellationToken cancellationToken = default);
    }
}
