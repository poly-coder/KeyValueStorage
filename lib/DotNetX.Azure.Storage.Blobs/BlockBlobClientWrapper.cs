using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlockBlobClientWrapper :
        BlobBaseClientWrapper,
        IBlockBlobClient
    {
        private readonly BlockBlobClient client;
        private readonly IBlockBlobClientWrapperFactory blockBlobClientFactory;

        public BlockBlobClientWrapper(
            BlockBlobClient client,
            IBlockBlobClientWrapperFactory blockBlobClientFactory) :
            base(client)
        {
            this.client = client;
            this.blockBlobClientFactory = blockBlobClientFactory;
        }

        protected override IBlobBaseClient WithSnapshotOverride(string snapshot) => WithSnapshot(snapshot);

        protected override IBlobBaseClient WithVersionOverride(string versionId) => WithVersion(versionId);

        protected override IBlobBaseClient WithCustomerProvidedKeyOverride(CustomerProvidedKey? customerProvidedKey) =>
            WithCustomerProvidedKey(customerProvidedKey);

        protected override IBlobBaseClient WithEncryptionScopeOverride(string encryptionScope) =>
            WithEncryptionScope(encryptionScope);

        public long BlockBlobMaxUploadBlobLongBytes => client.BlockBlobMaxUploadBlobLongBytes;
        public long BlockBlobMaxStageBlockLongBytes => client.BlockBlobMaxStageBlockLongBytes;
        public int BlockBlobMaxBlocks => client.BlockBlobMaxBlocks;

        public IBlockBlobClient WithSnapshot(string snapshot) =>
            blockBlobClientFactory.CreateWrapper(client.WithSnapshot(snapshot));

        public IBlockBlobClient WithVersion(string versionId) =>
            blockBlobClientFactory.CreateWrapper(client.WithVersion(versionId));

        public IBlockBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey) =>
            blockBlobClientFactory.CreateWrapper(client.WithCustomerProvidedKey(customerProvidedKey));

        public IBlockBlobClient WithEncryptionScope(string encryptionScope) =>
            blockBlobClientFactory.CreateWrapper(client.WithEncryptionScope(encryptionScope));

        public Response<BlobContentInfo> Upload(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.Upload(content, options, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(content, options, cancellationToken);

        public Response<BlockInfo> StageBlock(
            string base64BlockId,
            Stream content,
            byte[] transactionalContentHash = default,
            BlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default) =>
            client.StageBlock(
                base64BlockId, content, transactionalContentHash,
                conditions, progressHandler, cancellationToken);

        public Task<Response<BlockInfo>> StageBlockAsync(
            string base64BlockId,
            Stream content,
            byte[] transactionalContentHash = default,
            BlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default) =>
            client.StageBlockAsync(
                base64BlockId, content, transactionalContentHash,
                conditions, progressHandler, cancellationToken);

        public Response<BlockInfo> StageBlockFromUri(
            Uri sourceUri,
            string base64BlockId,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            RequestConditions sourceConditions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.StageBlockFromUri(
                sourceUri, base64BlockId, sourceRange, sourceContentHash,
                sourceConditions, conditions, cancellationToken);

        public Task<Response<BlockInfo>> StageBlockFromUriAsync(
            Uri sourceUri,
            string base64BlockId,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            RequestConditions sourceConditions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.StageBlockFromUriAsync(
                sourceUri, base64BlockId, sourceRange, sourceContentHash,
                sourceConditions, conditions, cancellationToken);

        public Response<BlobContentInfo> CommitBlockList(
            IEnumerable<string> base64BlockIds,
            CommitBlockListOptions options,
            CancellationToken cancellationToken = default) =>
            client.CommitBlockList(base64BlockIds, options, cancellationToken);

        public Task<Response<BlobContentInfo>> CommitBlockListAsync(
            IEnumerable<string> base64BlockIds,
            CommitBlockListOptions options,
            CancellationToken cancellationToken = default) =>
            client.CommitBlockListAsync(base64BlockIds, options, cancellationToken);

        public Response<BlockList> GetBlockList(
            BlockListTypes blockListTypes = BlockListTypes.All,
            string snapshot = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetBlockList(blockListTypes, snapshot, conditions, cancellationToken);

        public Task<Response<BlockList>> GetBlockListAsync(
            BlockListTypes blockListTypes = BlockListTypes.All,
            string snapshot = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetBlockListAsync(blockListTypes, snapshot, conditions, cancellationToken);

        public Response<BlobDownloadInfo> Query(
            string querySqlExpression,
            BlobQueryOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.Query(querySqlExpression, options, cancellationToken);

        public Task<Response<BlobDownloadInfo>> QueryAsync(
            string querySqlExpression,
            BlobQueryOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.QueryAsync(querySqlExpression, options, cancellationToken);

        public Stream OpenWrite(
            bool overwrite,
            BlockBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.OpenWrite(overwrite, options, cancellationToken);

        public Task<Stream> OpenWriteAsync(
            bool overwrite,
            BlockBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.OpenWriteAsync(overwrite, options, cancellationToken);

        public Response<BlobContentInfo> SyncUploadFromUri(
            Uri copySource,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.SyncUploadFromUri(copySource, overwrite, cancellationToken);

        public Task<Response<BlobContentInfo>> SyncUploadFromUriAsync(
            Uri copySource,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.SyncUploadFromUriAsync(copySource, overwrite, cancellationToken);

        public Response<BlobContentInfo> SyncUploadFromUri(
            Uri copySource,
            BlobSyncUploadFromUriOptions options,
            CancellationToken cancellationToken = default) =>
            client.SyncUploadFromUri(copySource, options, cancellationToken);

        public Task<Response<BlobContentInfo>> SyncUploadFromUriAsync(
            Uri copySource,
            BlobSyncUploadFromUriOptions options,
            CancellationToken cancellationToken = default) =>
            client.SyncUploadFromUriAsync(copySource, options, cancellationToken);
    }
}