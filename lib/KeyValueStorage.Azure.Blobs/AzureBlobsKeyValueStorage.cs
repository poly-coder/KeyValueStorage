using Azure;
using Azure.Storage.Blobs;
using KeyValueStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Metadata = System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>>;

namespace KeyValueStorage.Azure.Blobs
{
    public class AzureBlobsKeyValueStorage :
        LocalKeyValueStorageBase<string, byte[], Metadata>
    {
        private readonly BlobContainerClient containerClient;

        private const KeyValueStorageCapability Capabilities =
            KeyValueStorageCapability.Fetch |
            KeyValueStorageCapability.List |
            KeyValueStorageCapability.Store |
            KeyValueStorageCapability.AsyncList |
            KeyValueStorageCapability.KeyPrefix |
            KeyValueStorageCapability.Metadata;

        public static AzureBlobsKeyValueStorage CreateFrom(
            string connectionString,
            string containerName)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var service = new BlobServiceClient(connectionString);
            var containerClient = service.GetBlobContainerClient(containerName);

            return new AzureBlobsKeyValueStorage(containerClient);
        }

        public AzureBlobsKeyValueStorage(
            BlobContainerClient containerClient)
            : base(Capabilities)
        {
            this.containerClient = containerClient ?? throw new ArgumentNullException(nameof(containerClient));
        }

        #region [ IKeyValueMetadataFetcher ]

        public override async Task<KeyValueFetchResponse<byte[]>> FetchAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            try
            {
                var result = await blobClient.DownloadContentAsync(cancellationToken);

                var bytes = result.Value.Content.ToArray();

                return new KeyValueFetchResponse<byte[]>(true, bytes);
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new KeyValueFetchResponse<byte[]>(false, default);
            }
        }

        public override async Task<KeyValueMetadataFetchResponse<byte[], Metadata>> FetchMetadataAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            try
            {
                var result = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                var metadata = result.Value.Metadata;

                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    true, default, metadata);
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    false, default, default);
            }
        }

        public override async Task<KeyValueMetadataFetchResponse<byte[], Metadata>> FetchMetadataAndValueAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            try
            {
                var propertiesResult = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

                var metadata = propertiesResult.Value.Metadata;

                var result = await blobClient.DownloadContentAsync(cancellationToken);

                var bytes = result.Value.Content.ToArray();

                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    true, bytes, metadata);
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new KeyValueMetadataFetchResponse<byte[], Metadata>(
                    false, default, default);
            }
        }

        #endregion

        #region [ IKeyValueMetadataStorer ]

        public override async Task StoreAsync(
            string key,
            byte[] value,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            switch (storeMode)
            {
                case KeyValueStoreMode.CreateNew:
                    {
                        var existsResult = await blobClient.ExistsAsync(cancellationToken);

                        if (existsResult)
                        {
                            throw new InvalidOperationException($"Key '{key}' already exists");
                        }

                        break;
                    }

                case KeyValueStoreMode.ReplaceExisting:
                    {
                        var existsResult = await blobClient.ExistsAsync(cancellationToken);

                        if (!existsResult)
                        {
                            throw new InvalidOperationException($"Key '{key}' does not exist");
                        }

                        break;
                    }
            }

            var content = BinaryData.FromBytes(value);

            await blobClient.UploadAsync(
                content,
                cancellationToken);
        }

        public override async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public override async Task StoreMetadataAsync(
            string key,
            Metadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            switch (storeMode)
            {
                case KeyValueStoreMode.CreateNew:
                    {
                        throw new InvalidOperationException($"Cannot store metadata only when creating a new entry for key '{key}'");
                    }

                default:
                    {
                        // TODO: Avoid calling Exists by handling proper exception on SetMetadata
                        var existsResult = await blobClient.ExistsAsync(cancellationToken);

                        if (!existsResult)
                        {
                            throw new InvalidOperationException($"Key '{key}' does not exist");
                        }

                        break;
                    }
            }

            var metadataDict = new Dictionary<string, string>(metadata);

            await blobClient.SetMetadataAsync(metadataDict, cancellationToken: cancellationToken);
        }

        public override async Task StoreMetadataAndValueAsync(
            string key,
            byte[] value,
            Metadata metadata,
            KeyValueStoreMode storeMode = KeyValueStoreMode.CreateOrReplace,
            CancellationToken cancellationToken = default)
        {
            var blobClient = containerClient.GetBlobClient(key);

            switch (storeMode)
            {
                case KeyValueStoreMode.CreateNew:
                    {
                        var existsResult = await blobClient.ExistsAsync(cancellationToken);

                        if (existsResult)
                        {
                            throw new InvalidOperationException($"Key '{key}' already exists");
                        }

                        break;
                    }

                case KeyValueStoreMode.ReplaceExisting:
                    {
                        var existsResult = await blobClient.ExistsAsync(cancellationToken);

                        if (!existsResult)
                        {
                            throw new InvalidOperationException($"Key '{key}' does not exist");
                        }

                        break;
                    }
            }

            var content = BinaryData.FromBytes(value);

            var options = new BlobUploadOptions()
            {
                Metadata = new Dictionary<string, string>(metadata),
            };

            await blobClient.UploadAsync(
                content,
                options,
                cancellationToken);
        }
        #endregion

        #region [ IKeyAsyncMetadataLister ]

        public override async Task<ICollection<KeyListerItem<string>>> ListKeysAsync(CancellationToken cancellationToken)
        {
            return await ListAsyncKeys(cancellationToken)
                .SelectMany(page => page.ToAsyncEnumerable())
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public override async Task<ICollection<KeyMetadataListerItem<string, Metadata>>> ListMetadataKeysAsync(CancellationToken cancellationToken)
        {
            return await ListAsyncMetadataKeys(cancellationToken)
                .SelectMany(page => page.ToAsyncEnumerable())
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public override async IAsyncEnumerable<ICollection<KeyListerItem<string>>> ListAsyncKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var pageable = containerClient.GetBlobsAsync(
                BlobTraits.None,
                BlobStates.None,
                prefix: null,
                cancellationToken);

            await foreach (var page in pageable.AsPages().WithCancellation(cancellationToken))
            {
                yield return page.Values
                    .Select(item => new KeyListerItem<string>(item.Name))
                    .ToList();
            }
        }

        public override async IAsyncEnumerable<ICollection<KeyMetadataListerItem<string, Metadata>>> ListAsyncMetadataKeys(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var pageable = containerClient.GetBlobsAsync(
                BlobTraits.Metadata,
                BlobStates.None,
                prefix: null,
                cancellationToken);

            await foreach (var page in pageable.AsPages().WithCancellation(cancellationToken))
            {
                yield return page.Values
                    .Select(item => new KeyMetadataListerItem<string, Metadata>(item.Name, item.Metadata))
                    .ToList();
            }
        }

        #endregion

        #region [ IKeyPrefixAsyncMetadataLister ]

        public override async Task<ICollection<KeyListerItem<string>>> ListPrefixedKeysAsync(
            string keyPrefix, 
            CancellationToken cancellationToken)
        {
            return await ListAsyncPrefixedKeys(keyPrefix, cancellationToken)
                .SelectMany(page => page.ToAsyncEnumerable())
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public override async Task<ICollection<KeyMetadataListerItem<string, Metadata>>> ListPrefixedMetadataKeysAsync(
            string keyPrefix, 
            CancellationToken cancellationToken)
        {
            return await ListAsyncPrefixedMetadataKeys(keyPrefix, cancellationToken)
                .SelectMany(page => page.ToAsyncEnumerable())
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public override async IAsyncEnumerable<ICollection<KeyListerItem<string>>> ListAsyncPrefixedKeys(
            string keyPrefix, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var pageable = containerClient.GetBlobsAsync(
                BlobTraits.None,
                BlobStates.None,
                keyPrefix,
                cancellationToken);

            await foreach (var page in pageable.AsPages().WithCancellation(cancellationToken))
            {
                yield return page.Values
                    .Select(item => new KeyListerItem<string>(item.Name))
                    .ToList();
            }
        }

        public override async IAsyncEnumerable<ICollection<KeyMetadataListerItem<string, Metadata>>> ListAsyncPrefixedMetadataKeys(
            string keyPrefix, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var pageable = containerClient.GetBlobsAsync(
                BlobTraits.Metadata,
                BlobStates.None,
                keyPrefix,
                cancellationToken);

            await foreach (var page in pageable.AsPages().WithCancellation(cancellationToken))
            {
                yield return page.Values
                    .Select(item => new KeyMetadataListerItem<string, Metadata>(item.Name, item.Metadata))
                    .ToList();
            }
        }

        #endregion
    }
}
