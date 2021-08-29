using Azure;
using Azure.Storage;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobBaseClient
    {
        Uri Uri { get; }

        string AccountName { get; }

        string BlobContainerName { get; }

        string Name { get; }

        bool CanGenerateSasUri { get; }

        IBlobBaseClient WithSnapshot(string snapshot);

        IBlobBaseClient WithVersion(string versionId);

        IBlobBaseClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey);

        IBlobBaseClient WithEncryptionScope(string encryptionScope);

        Response<BlobDownloadStreamingResult> DownloadStreaming(
            HttpRange range = default,
            BlobRequestConditions conditions = default,
            bool rangeGetContentHash = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobDownloadStreamingResult>> DownloadStreamingAsync(
            HttpRange range = default,
            BlobRequestConditions conditions = default,
            bool rangeGetContentHash = default,
            CancellationToken cancellationToken = default);

        Response<BlobDownloadResult> DownloadContent();

        Task<Response<BlobDownloadResult>> DownloadContentAsync();

        Response<BlobDownloadResult> DownloadContent(
            CancellationToken cancellationToken);

        Task<Response<BlobDownloadResult>> DownloadContentAsync(
            CancellationToken cancellationToken);

        Response<BlobDownloadResult> DownloadContent(
            BlobRequestConditions conditions,
            CancellationToken cancellationToken = default);

        Task<Response<BlobDownloadResult>> DownloadContentAsync(
            BlobRequestConditions conditions,
            CancellationToken cancellationToken = default);

        Response DownloadTo(Stream destination);

        Response DownloadTo(string path);

        Task<Response> DownloadToAsync(Stream destination);

        Task<Response> DownloadToAsync(string path);

        Response DownloadTo(
            Stream destination,
            CancellationToken cancellationToken);

        Response DownloadTo(
            string path,
            CancellationToken cancellationToken);

        Task<Response> DownloadToAsync(
            Stream destination,
            CancellationToken cancellationToken);

        Task<Response> DownloadToAsync(
            string path,
            CancellationToken cancellationToken);

        Response DownloadTo(
            Stream destination,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default);

        Response DownloadTo(
            string path,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DownloadToAsync(
            Stream destination,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DownloadToAsync(
            string path,
            BlobRequestConditions conditions = default,
            StorageTransferOptions transferOptions = default,
            CancellationToken cancellationToken = default);

        Stream OpenRead(
            BlobOpenReadOptions options,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenReadAsync(
            BlobOpenReadOptions options,
            CancellationToken cancellationToken = default);

        Stream OpenRead(
            long position = 0,
            int? bufferSize = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Stream OpenRead(
            bool allowBlobModifications,
            long position = 0,
            int? bufferSize = default,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenReadAsync(
            long position = 0,
            int? bufferSize = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenReadAsync(
            bool allowBlobModifications,
            long position = 0,
            int? bufferSize = default,
            CancellationToken cancellationToken = default);

        CopyFromUriOperation StartCopyFromUri(
            Uri source,
            BlobCopyFromUriOptions options,
            CancellationToken cancellationToken = default);

        CopyFromUriOperation StartCopyFromUri(
            Uri source,
            IDictionary<string, string> metadata = default,
            AccessTier? accessTier = default,
            BlobRequestConditions sourceConditions = default,
            BlobRequestConditions destinationConditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default);

        Task<CopyFromUriOperation> StartCopyFromUriAsync(
            Uri source,
            BlobCopyFromUriOptions options,
            CancellationToken cancellationToken = default);

        Task<CopyFromUriOperation> StartCopyFromUriAsync(
            Uri source,
            IDictionary<string, string> metadata = default,
            AccessTier? accessTier = default,
            BlobRequestConditions sourceConditions = default,
            BlobRequestConditions destinationConditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default);

        Response AbortCopyFromUri(
            string copyId,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> AbortCopyFromUriAsync(
            string copyId,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobCopyInfo> SyncCopyFromUri(
            Uri source,
            BlobCopyFromUriOptions options = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobCopyInfo>> SyncCopyFromUriAsync(
            Uri source,
            BlobCopyFromUriOptions options = default,
            CancellationToken cancellationToken = default);

        Response Delete(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DeleteAsync(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<bool> DeleteIfExists(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<bool>> DeleteIfExistsAsync(
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<bool> Exists(
            CancellationToken cancellationToken = default);

        Task<Response<bool>> ExistsAsync(
            CancellationToken cancellationToken = default);

        Response Undelete(
            CancellationToken cancellationToken = default);

        Task<Response> UndeleteAsync(
            CancellationToken cancellationToken = default);

        Response<BlobProperties> GetProperties(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobProperties>> GetPropertiesAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobInfo> SetHttpHeaders(
            BlobHttpHeaders httpHeaders = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobInfo>> SetHttpHeadersAsync(
            BlobHttpHeaders httpHeaders = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobInfo> SetMetadata(
            IDictionary<string, string> metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobInfo>> SetMetadataAsync(
            IDictionary<string, string> metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobSnapshotInfo> CreateSnapshot(
            IDictionary<string, string> metadata = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobSnapshotInfo>> CreateSnapshotAsync(
            IDictionary<string, string> metadata = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response SetAccessTier(
            AccessTier accessTier,
            BlobRequestConditions conditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default);

        Task<Response> SetAccessTierAsync(
            AccessTier accessTier,
            BlobRequestConditions conditions = default,
            RehydratePriority? rehydratePriority = default,
            CancellationToken cancellationToken = default);

        Response<GetBlobTagResult> GetTags(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<GetBlobTagResult>> GetTagsAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response SetTags(
            IDictionary<string, string> tags,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> SetTagsAsync(
            IDictionary<string, string> tags,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Uri GenerateSasUri(BlobSasPermissions permissions, DateTimeOffset expiresOn);

        Uri GenerateSasUri(BlobSasBuilder builder);
    }
}