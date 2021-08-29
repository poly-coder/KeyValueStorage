using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Metadata = System.Collections.Generic.IDictionary<string, string>;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobContainerClient
    {
        Uri Uri { get; }

        string AccountName { get; }

        string Name { get; }

        bool CanGenerateSasUri { get; }

        IBlobClient GetBlobClient(string blobName);

        IBlockBlobClient GetBlockBlobClient(string blobName);

        IPageBlobClient GetPageBlobClient(string blobName);

        IAppendBlobClient GetAppendBlobClient(string blobName);

        Response<BlobContainerInfo> Create(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerInfo>> CreateAsync(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContainerInfo> CreateIfNotExists(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerInfo>> CreateIfNotExistsAsync(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default);

        Response Delete(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DeleteAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<bool> DeleteIfExists(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<bool>> DeleteIfExistsAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<bool> Exists(
            CancellationToken cancellationToken = default);

        Task<Response<bool>> ExistsAsync(
            CancellationToken cancellationToken = default);

        Response<BlobContainerProperties> GetProperties(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerProperties>> GetPropertiesAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContainerInfo> SetMetadata(
            Metadata metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerInfo>> SetMetadataAsync(
            Metadata metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContainerAccessPolicy> GetAccessPolicy(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerAccessPolicy>> GetAccessPolicyAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContainerInfo> SetAccessPolicy(
            PublicAccessType accessType = PublicAccessType.None,
            IEnumerable<BlobSignedIdentifier> permissions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerInfo>> SetAccessPolicyAsync(
            PublicAccessType accessType = PublicAccessType.None,
            IEnumerable<BlobSignedIdentifier> permissions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        IPageable<BlobItem> GetBlobs(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default);

        IAsyncPageable<BlobItem> GetBlobsAsync(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default);

        IPageable<BlobHierarchyItem> GetBlobsByHierarchy(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string delimiter = default,
            string prefix = default,
            CancellationToken cancellationToken = default);

        IAsyncPageable<BlobHierarchyItem> GetBlobsByHierarchyAsync(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string delimiter = default,
            string prefix = default,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> UploadBlob(
            string blobName,
            Stream content,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadBlobAsync(
            string blobName,
            Stream content,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> UploadBlob(
            string blobName,
            BinaryData content,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadBlobAsync(
            string blobName,
            BinaryData content,
            CancellationToken cancellationToken = default);

        Response DeleteBlob(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DeleteBlobAsync(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<bool> DeleteBlobIfExists(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<bool>> DeleteBlobIfExistsAsync(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Uri GenerateSasUri(BlobContainerSasPermissions permissions, DateTimeOffset expiresOn);

        Uri GenerateSasUri(BlobSasBuilder builder);
    }
}
