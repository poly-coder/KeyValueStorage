using System.Threading;
using System.Threading.Tasks;

namespace KeyValueStorage.Abstractions
{
    public interface IKeyValueStorage
    {
        Task<KeyValueStorageCapability> GetCapabilitiesAsync(
            CancellationToken cancellationToken = default);
    }
}
