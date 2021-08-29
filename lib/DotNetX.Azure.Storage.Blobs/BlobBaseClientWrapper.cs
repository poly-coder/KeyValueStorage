using Azure;
using Azure.Storage;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public abstract class BlobBaseClientWrapper : IBlobBaseClient
    {
        private readonly BlobBaseClient client;

        protected BlobBaseClientWrapper(BlobBaseClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Uri Uri => client.Uri;
        public string AccountName => client.AccountName;
        public string BlobContainerName => client.BlobContainerName;
        public string Name => client.Name;
        public bool CanGenerateSasUri => client.CanGenerateSasUri;

        IBlobBaseClient IBlobBaseClient.WithSnapshot(string snapshot) => 
            WithSnapshotOverride(snapshot);

        IBlobBaseClient IBlobBaseClient.WithVersion(string versionId) => 
            WithVersionOverride(versionId);

        IBlobBaseClient IBlobBaseClient.WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey) =>
            WithCustomerProvidedKeyOverride(customerProvidedKey);

        IBlobBaseClient IBlobBaseClient.WithEncryptionScope(string encryptionScope) =>
            WithEncryptionScopeOverride(encryptionScope);

        protected abstract IBlobBaseClient WithSnapshotOverride(string snapshot);

        protected abstract IBlobBaseClient WithVersionOverride(string versionId);

        protected abstract IBlobBaseClient WithCustomerProvidedKeyOverride(CustomerProvidedKey? customerProvidedKey);

        protected abstract IBlobBaseClient WithEncryptionScopeOverride(string encryptionScope);

        public Response<BlobDownloadStreamingResult> DownloadStreaming(
            HttpRange range = default,
            BlobRequestConditions conditions = default,
            bool rangeGetContentHash = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadStreaming(range, conditions, rangeGetContentHash, cancellationToken);
        }

        public Task<Response<BlobDownloadStreamingResult>> DownloadStreamingAsync(
            HttpRange range = default,
            BlobRequestConditions conditions = default,
            bool rangeGetContentHash = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadStreamingAsync(range, conditions, rangeGetContentHash, cancellationToken);
        }

        public Response<BlobDownloadResult> DownloadContent()
        {
            return client.DownloadContent();
        }

        public Task<Response<BlobDownloadResult>> DownloadContentAsync()
        {
            return client.DownloadContentAsync();
        }

        public Response<BlobDownloadResult> DownloadContent(
            CancellationToken cancellationToken)
        {
            return client.DownloadContent(cancellationToken);
        }

        public Task<Response<BlobDownloadResult>> DownloadContentAsync(
            CancellationToken cancellationToken)
        {
            return client.DownloadContentAsync(cancellationToken);
        }

        public Response<BlobDownloadResult> DownloadContent(
            BlobRequestConditions conditions,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadContent(conditions, cancellationToken);
        }

        public Task<Response<BlobDownloadResult>> DownloadContentAsync(
            BlobRequestConditions conditions,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadContentAsync(conditions, cancellationToken);
        }

        public Response DownloadTo(Stream destination)
        {
            return client.DownloadTo(destination);
        }

        public Response DownloadTo(string path)
        {
            return client.DownloadTo(path);
        }

        public Task<Response> DownloadToAsync(Stream destination)
        {
            return client.DownloadToAsync(destination);
        }

        public Task<Response> DownloadToAsync(string path)
        {
            return client.DownloadToAsync(path);
        }

        public Response DownloadTo(
            Stream destination,
            CancellationToken cancellationToken)
        {
            return client.DownloadTo(destination, cancellationToken);
        }

        public Response DownloadTo(
            string path,
            CancellationToken cancellationToken)
        {
            return client.DownloadTo(path, cancellationToken);
        }

        public Task<Response> DownloadToAsync(
            Stream destination,
            CancellationToken cancellationToken)
        {
            return client.DownloadToAsync(destination, cancellationToken);
        }

        public Task<Response> DownloadToAsync(
            string path,
            CancellationToken cancellationToken)
        {
            return client.DownloadToAsync(path, cancellationToken);
        }

        public Response DownloadTo(
            Stream destination,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadTo(destination, conditions, transferOptions, cancellationToken);
        }

        public Response DownloadTo(
            string path,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadTo(path, conditions, transferOptions, cancellationToken);
        }

        public Task<Response> DownloadToAsync(
            Stream destination,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadToAsync(destination, conditions, transferOptions, cancellationToken);
        }

        public Task<Response> DownloadToAsync(
            string path,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DownloadToAsync(path, conditions, transferOptions, cancellationToken);
        }

        public Stream OpenRead(
            BlobOpenReadOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.OpenRead(options, cancellationToken);
        }

        public Task<Stream> OpenReadAsync(
            BlobOpenReadOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.OpenReadAsync(options, cancellationToken);
        }

        public Stream OpenRead(
            long position = 0,
            int? bufferSize = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenRead(position, bufferSize, conditions, cancellationToken);
        }

        public Stream OpenRead(
            bool allowBlobModifications,
            long position = 0,
            int? bufferSize = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenRead(allowBlobModifications, position, bufferSize, cancellationToken);
        }

        public Task<Stream> OpenReadAsync(
            long position = 0,
            int? bufferSize = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenReadAsync(position, bufferSize, conditions, cancellationToken);
        }

        public Task<Stream> OpenReadAsync(
            bool allowBlobModifications,
            long position = 0,
            int? bufferSize = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenReadAsync(allowBlobModifications, position, bufferSize, cancellationToken);
        }

        public CopyFromUriOperation StartCopyFromUri(
            Uri source,
            BlobCopyFromUriOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.StartCopyFromUri(source, options, cancellationToken);
        }

        public CopyFromUriOperation StartCopyFromUri(
            Uri source,
            IDictionary<string, string> metadata = default,
            AccessTier? accessTier = default,
            BlobRequestConditions sourceConditions = default,
            BlobRequestConditions destinationConditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default)
        {
            return client.StartCopyFromUri(
                source, metadata, accessTier,
                sourceConditions, destinationConditions,
                rehydratePriority, cancellationToken);
        }

        public Task<CopyFromUriOperation> StartCopyFromUriAsync(
            Uri source,
            BlobCopyFromUriOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.StartCopyFromUriAsync(source, options, cancellationToken);
        }

        public Task<CopyFromUriOperation> StartCopyFromUriAsync(
            Uri source,
            IDictionary<string, string> metadata = default,
            AccessTier? accessTier = default,
            BlobRequestConditions sourceConditions = default,
            BlobRequestConditions destinationConditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default)
        {
            return client.StartCopyFromUriAsync(
                source, metadata, accessTier,
                sourceConditions, destinationConditions,
                rehydratePriority, cancellationToken);
        }

        public Response AbortCopyFromUri(
            string copyId,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.AbortCopyFromUri(copyId, conditions, cancellationToken);
        }

        public Task<Response> AbortCopyFromUriAsync(
            string copyId,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.AbortCopyFromUriAsync(copyId, conditions, cancellationToken);
        }

        public Response<BlobCopyInfo> SyncCopyFromUri(
            Uri source,
            BlobCopyFromUriOptions options = default,
            CancellationToken cancellationToken = default)
        {
            return client.SyncCopyFromUri(source, options, cancellationToken);
        }

        public Task<Response<BlobCopyInfo>> SyncCopyFromUriAsync(
            Uri source,
            BlobCopyFromUriOptions options = default,
            CancellationToken cancellationToken = default)
        {
            return client.SyncCopyFromUriAsync(source, options, cancellationToken);
        }

        public Response Delete(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.Delete(snapshotsOption, conditions, cancellationToken);
        }

        public Task<Response> DeleteAsync(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DeleteAsync(snapshotsOption, conditions, cancellationToken);
        }

        public Response<bool> DeleteIfExists(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DeleteIfExists(snapshotsOption, conditions, cancellationToken);
        }

        public Task<Response<bool>> DeleteIfExistsAsync(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.DeleteIfExistsAsync(snapshotsOption, conditions, cancellationToken);
        }

        public Response<bool> Exists(CancellationToken cancellationToken = default)
        {
            return client.Exists(cancellationToken);
        }

        public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken = default)
        {
            return client.ExistsAsync(cancellationToken);
        }

        public Response Undelete(CancellationToken cancellationToken = default)
        {
            return client.Undelete(cancellationToken);
        }

        public Task<Response> UndeleteAsync(CancellationToken cancellationToken = default)
        {
            return client.UndeleteAsync(cancellationToken);
        }

        public Response<BlobProperties> GetProperties(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.GetProperties(conditions, cancellationToken);
        }

        public Task<Response<BlobProperties>> GetPropertiesAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.GetPropertiesAsync(conditions, cancellationToken);
        }

        public Response<BlobInfo> SetHttpHeaders(
            BlobHttpHeaders httpHeaders = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetHttpHeaders(httpHeaders, conditions, cancellationToken);
        }

        public Task<Response<BlobInfo>> SetHttpHeadersAsync(
            BlobHttpHeaders httpHeaders = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetHttpHeadersAsync(httpHeaders, conditions, cancellationToken);
        }

        public Response<BlobInfo> SetMetadata(
            IDictionary<string, string> metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetMetadata(metadata, conditions, cancellationToken);
        }

        public Task<Response<BlobInfo>> SetMetadataAsync(
            IDictionary<string, string> metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetMetadataAsync(metadata, conditions, cancellationToken);
        }

        public Response<BlobSnapshotInfo> CreateSnapshot(
            IDictionary<string, string> metadata = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.CreateSnapshot(metadata, conditions, cancellationToken);
        }

        public Task<Response<BlobSnapshotInfo>> CreateSnapshotAsync(
            IDictionary<string, string> metadata = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.CreateSnapshotAsync(metadata, conditions, cancellationToken);
        }

        public Response SetAccessTier(
            AccessTier accessTier,
            BlobRequestConditions conditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetAccessTier(accessTier, conditions, rehydratePriority, cancellationToken);
        }

        public Task<Response> SetAccessTierAsync(
            AccessTier accessTier,
            BlobRequestConditions conditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetAccessTierAsync(accessTier, conditions, rehydratePriority, cancellationToken);
        }

        public Response<GetBlobTagResult> GetTags(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.GetTags(conditions, cancellationToken);
        }

        public Task<Response<GetBlobTagResult>> GetTagsAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.GetTagsAsync(conditions, cancellationToken);
        }

        public Response SetTags(
            IDictionary<string, string> tags,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetTags(tags, conditions, cancellationToken);
        }

        public Task<Response> SetTagsAsync(
            IDictionary<string, string> tags,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SetTagsAsync(tags, conditions, cancellationToken);
        }

        public Uri GenerateSasUri(
            BlobSasPermissions permissions,
            DateTimeOffset expiresOn)
        {
            return client.GenerateSasUri(permissions, expiresOn);
        }

        public Uri GenerateSasUri(BlobSasBuilder builder)
        {
            return client.GenerateSasUri(builder);
        }
    }
}