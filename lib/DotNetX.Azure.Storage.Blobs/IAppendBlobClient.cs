using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IAppendBlobClient : IBlobBaseClient
    {
        int AppendBlobMaxAppendBlockBytes { get; }

        int AppendBlobMaxBlocks { get; }

        new IAppendBlobClient WithSnapshot(string snapshot);

        new IAppendBlobClient WithVersion(string versionId);

        new IAppendBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey);

        new IAppendBlobClient WithEncryptionScope(string encryptionScope);

        Response<BlobContentInfo> Create(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> CreateAsync(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> CreateIfNotExists(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> CreateIfNotExistsAsync(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Response<BlobAppendInfo> AppendBlock(
            Stream content,
            byte[] transactionalContentHash = default,
            AppendBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobAppendInfo>> AppendBlockAsync(
            Stream content,
            byte[] transactionalContentHash = default,
            AppendBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Response<BlobAppendInfo> AppendBlockFromUri(
            Uri sourceUri,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            AppendBlobRequestConditions conditions = default,
            AppendBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobAppendInfo>> AppendBlockFromUriAsync(
            Uri sourceUri,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            AppendBlobRequestConditions conditions = default,
            AppendBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobInfo> Seal(
            AppendBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobInfo>> SealAsync(
            AppendBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Stream OpenWrite(
            bool overwrite,
            AppendBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenWriteAsync(
            bool overwrite,
            AppendBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);
    }
}