using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Metadata = System.Collections.Generic.IDictionary<string, string>;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobContainerClientWrapper :
        IBlobContainerClient
    {
        private readonly BlobContainerClient client;
        private readonly IBlobClientWrapperFactory blobClientFactory;
        private readonly IBlockBlobClientWrapperFactory blockBlobClientFactory;
        private readonly IAppendBlobClientWrapperFactory appendBlobClientFactory;
        private readonly IPageBlobClientWrapperFactory pageBlobClientFactory;
        private readonly IPageableWrapperFactory pageableFactory;
        private readonly IAsyncPageableWrapperFactory asyncPageableFactory;

        public BlobContainerClientWrapper(
            BlobContainerClient client,
            IBlobClientWrapperFactory blobClientFactory,
            IBlockBlobClientWrapperFactory blockBlobClientFactory,
            IAppendBlobClientWrapperFactory appendBlobClientFactory,
            IPageBlobClientWrapperFactory pageBlobClientFactory,
            IPageableWrapperFactory pageableFactory,
            IAsyncPageableWrapperFactory asyncPageableFactory)
        {
            this.client = client;
            this.blobClientFactory = blobClientFactory;
            this.blockBlobClientFactory = blockBlobClientFactory;
            this.appendBlobClientFactory = appendBlobClientFactory;
            this.pageBlobClientFactory = pageBlobClientFactory;
            this.pageableFactory = pageableFactory;
            this.asyncPageableFactory = asyncPageableFactory;
        }

        public Uri Uri => client.Uri;
        public string AccountName => client.AccountName;
        public string Name => client.Name;
        public bool CanGenerateSasUri => client.CanGenerateSasUri;

        public IBlobClient GetBlobClient(string blobName) =>
            blobClientFactory.CreateWrapper(client.GetBlobClient(blobName));

        public IBlockBlobClient GetBlockBlobClient(string blobName) =>
            blockBlobClientFactory.CreateWrapper(client.GetBlockBlobClient(blobName));

        public IPageBlobClient GetPageBlobClient(string blobName) =>
            pageBlobClientFactory.CreateWrapper(client.GetPageBlobClient(blobName));

        public IAppendBlobClient GetAppendBlobClient(string blobName) =>
            appendBlobClientFactory.CreateWrapper(client.GetAppendBlobClient(blobName));

        public Response<BlobContainerInfo> Create(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default) =>
            client.Create(publicAccessType, metadata, encryptionScopeOptions, cancellationToken);

        public Task<Response<BlobContainerInfo>> CreateAsync(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default) =>
            client.CreateAsync(publicAccessType, metadata, encryptionScopeOptions, cancellationToken);

        public Response<BlobContainerInfo> CreateIfNotExists(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default) =>
            client.CreateIfNotExists(publicAccessType, metadata, encryptionScopeOptions, cancellationToken);

        public Task<Response<BlobContainerInfo>> CreateIfNotExistsAsync(
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            BlobContainerEncryptionScopeOptions encryptionScopeOptions = default,
            CancellationToken cancellationToken = default) =>
            client.CreateIfNotExistsAsync(publicAccessType, metadata, encryptionScopeOptions, cancellationToken);

        public Response Delete(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.Delete(conditions, cancellationToken);

        public Task<Response> DeleteAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteAsync(conditions, cancellationToken);

        public Response<bool> DeleteIfExists(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteIfExists(conditions, cancellationToken);

        public Task<Response<bool>> DeleteIfExistsAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteIfExistsAsync(conditions, cancellationToken);

        public Response<bool> Exists(CancellationToken cancellationToken = default) =>
            client.Exists(cancellationToken);

        public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken = default) =>
            client.ExistsAsync(cancellationToken);

        public Response<BlobContainerProperties> GetProperties(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetProperties(conditions, cancellationToken);

        public Task<Response<BlobContainerProperties>> GetPropertiesAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetPropertiesAsync(conditions, cancellationToken);

        public Response<BlobContainerInfo> SetMetadata(
            Metadata metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.SetMetadata(metadata, conditions, cancellationToken);

        public Task<Response<BlobContainerInfo>> SetMetadataAsync(
            Metadata metadata,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.SetMetadataAsync(metadata, conditions, cancellationToken);

        public Response<BlobContainerAccessPolicy> GetAccessPolicy(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetAccessPolicy(conditions, cancellationToken);

        public Task<Response<BlobContainerAccessPolicy>> GetAccessPolicyAsync(
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetAccessPolicyAsync(conditions, cancellationToken);

        public Response<BlobContainerInfo> SetAccessPolicy(
            PublicAccessType accessType = PublicAccessType.None,
            IEnumerable<BlobSignedIdentifier> permissions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.SetAccessPolicy(
                accessType, permissions, conditions, cancellationToken);

        public Task<Response<BlobContainerInfo>> SetAccessPolicyAsync(
            PublicAccessType accessType = PublicAccessType.None,
            IEnumerable<BlobSignedIdentifier> permissions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.SetAccessPolicyAsync(
                accessType, permissions, conditions, cancellationToken);

        public IPageable<BlobItem> GetBlobs(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            pageableFactory.CreateWrapper(
                client.GetBlobs(
                    traits, states, prefix, cancellationToken));

        public IAsyncPageable<BlobItem> GetBlobsAsync(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            asyncPageableFactory.CreateWrapper(
                client.GetBlobsAsync(
                    traits, states, prefix, cancellationToken));

        public IPageable<BlobHierarchyItem> GetBlobsByHierarchy(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string delimiter = default,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            pageableFactory.CreateWrapper(
                client.GetBlobsByHierarchy(
                    traits, states, delimiter, prefix, cancellationToken));

        public IAsyncPageable<BlobHierarchyItem> GetBlobsByHierarchyAsync(
            BlobTraits traits = BlobTraits.None,
            BlobStates states = BlobStates.None,
            string delimiter = default,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            asyncPageableFactory.CreateWrapper(
                client.GetBlobsByHierarchyAsync(
                    traits, states, delimiter, prefix, cancellationToken));

        public Response<BlobContentInfo> UploadBlob(
            string blobName,
            Stream content,
            CancellationToken cancellationToken = default) =>
            client.UploadBlob(blobName, content, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadBlobAsync(
            string blobName,
            Stream content,
            CancellationToken cancellationToken = default) =>
            client.UploadBlobAsync(blobName, content, cancellationToken);

        public Response<BlobContentInfo> UploadBlob(
            string blobName,
            BinaryData content,
            CancellationToken cancellationToken = default) =>
            client.UploadBlob(blobName, content, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadBlobAsync(
            string blobName,
            BinaryData content,
            CancellationToken cancellationToken = default) =>
            client.UploadBlobAsync(blobName, content, cancellationToken);

        public Response DeleteBlob(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlob(
                blobName, snapshotsOption, conditions, cancellationToken);

        public Task<Response> DeleteBlobAsync(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlobAsync(
                blobName, snapshotsOption, conditions, cancellationToken);

        public Response<bool> DeleteBlobIfExists(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlobIfExists(
                blobName, snapshotsOption, conditions, cancellationToken);

        public Task<Response<bool>> DeleteBlobIfExistsAsync(
            string blobName,
            DeleteSnapshotsOption snapshotsOption = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlobIfExistsAsync(
                blobName, snapshotsOption, conditions, cancellationToken);

        public Uri GenerateSasUri(
            BlobContainerSasPermissions permissions,
            DateTimeOffset expiresOn) =>
            client.GenerateSasUri(permissions, expiresOn);

        public Uri GenerateSasUri(BlobSasBuilder builder) =>
            client.GenerateSasUri(builder);
    }
}
