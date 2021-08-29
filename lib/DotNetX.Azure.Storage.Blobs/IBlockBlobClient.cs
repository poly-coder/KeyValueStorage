using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlockBlobClient : IBlobBaseClient
    {
        long BlockBlobMaxUploadBlobLongBytes { get; }

        long BlockBlobMaxStageBlockLongBytes { get; }

        int BlockBlobMaxBlocks { get; }

        new IBlockBlobClient WithSnapshot(string snapshot);

        new IBlockBlobClient WithVersion(string versionId);

        new IBlockBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey);

        new IBlockBlobClient WithEncryptionScope(string encryptionScope);

        Response<BlobContentInfo> Upload(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Response<BlockInfo> StageBlock(
            string base64BlockId,
            Stream content,
            byte[] transactionalContentHash = default,
            BlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlockInfo>> StageBlockAsync(
            string base64BlockId,
            Stream content,
            byte[] transactionalContentHash = default,
            BlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Response<BlockInfo> StageBlockFromUri(
            Uri sourceUri,
            string base64BlockId,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            RequestConditions sourceConditions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlockInfo>> StageBlockFromUriAsync(
            Uri sourceUri,
            string base64BlockId,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            RequestConditions sourceConditions = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> CommitBlockList(
            IEnumerable<string> base64BlockIds,
            CommitBlockListOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> CommitBlockListAsync(
            IEnumerable<string> base64BlockIds,
            CommitBlockListOptions options,
            CancellationToken cancellationToken = default);

        Response<BlockList> GetBlockList(
            BlockListTypes blockListTypes = BlockListTypes.All,
            string snapshot = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlockList>> GetBlockListAsync(
            BlockListTypes blockListTypes = BlockListTypes.All,
            string snapshot = default,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobDownloadInfo> Query(
            string querySqlExpression,
            BlobQueryOptions options = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobDownloadInfo>> QueryAsync(
            string querySqlExpression,
            BlobQueryOptions options = default,
            CancellationToken cancellationToken = default);

        Stream OpenWrite(
            bool overwrite,
            BlockBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenWriteAsync(
            bool overwrite,
            BlockBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> SyncUploadFromUri(
            Uri copySource,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> SyncUploadFromUriAsync(
            Uri copySource,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> SyncUploadFromUri(
            Uri copySource,
            BlobSyncUploadFromUriOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> SyncUploadFromUriAsync(
            Uri copySource,
            BlobSyncUploadFromUriOptions options,
            CancellationToken cancellationToken = default);
    }
}