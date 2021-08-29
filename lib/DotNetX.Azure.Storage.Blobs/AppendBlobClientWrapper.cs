using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public class AppendBlobClientWrapper :
        BlobBaseClientWrapper,
        IAppendBlobClient
    {
        private readonly AppendBlobClient client;
        private readonly IAppendBlobClientWrapperFactory appendBlobFactory;

        public AppendBlobClientWrapper(
            AppendBlobClient client,
            IAppendBlobClientWrapperFactory appendBlobFactory) :
            base(client)
        {
            this.client = client;
            this.appendBlobFactory = appendBlobFactory;
        }

        public int AppendBlobMaxAppendBlockBytes => client.AppendBlobMaxAppendBlockBytes;
        public int AppendBlobMaxBlocks => client.AppendBlobMaxBlocks;

        public IAppendBlobClient WithSnapshot(string snapshot)
        {
            return appendBlobFactory.CreateWrapper(client.WithSnapshot(snapshot));
        }

        public IAppendBlobClient WithVersion(string versionId)
        {
            return appendBlobFactory.CreateWrapper(client.WithVersion(versionId));
        }

        public IAppendBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey)
        {
            return appendBlobFactory.CreateWrapper(client.WithCustomerProvidedKey(customerProvidedKey));
        }

        public IAppendBlobClient WithEncryptionScope(string encryptionScope)
        {
            return appendBlobFactory.CreateWrapper(client.WithEncryptionScope(encryptionScope));
        }

        public Response<BlobContentInfo> Create(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.Create(options, cancellationToken);
        }

        public Task<Response<BlobContentInfo>> CreateAsync(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.CreateAsync(options, cancellationToken);
        }

        public Response<BlobContentInfo> CreateIfNotExists(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.CreateIfNotExists(options, cancellationToken);
        }

        public Task<Response<BlobContentInfo>> CreateIfNotExistsAsync(
            AppendBlobCreateOptions options,
            CancellationToken cancellationToken = default)
        {
            return client.CreateIfNotExistsAsync(options, cancellationToken);
        }

        public Response<BlobAppendInfo> AppendBlock(
            Stream content,
            byte[] transactionalContentHash = default,
            AppendBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default)
        {
            return client.AppendBlock(
                content, transactionalContentHash,
                conditions, progressHandler, cancellationToken);
        }

        public Task<Response<BlobAppendInfo>> AppendBlockAsync(
            Stream content,
            byte[] transactionalContentHash = default,
            AppendBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default)
        {
            return client.AppendBlockAsync(
                content, transactionalContentHash,
                conditions, progressHandler, cancellationToken);
        }

        public Response<BlobAppendInfo> AppendBlockFromUri(
            Uri sourceUri,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            AppendBlobRequestConditions conditions = default,
            AppendBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.AppendBlockFromUri(
                sourceUri, sourceRange, sourceContentHash,
                conditions, sourceConditions, cancellationToken);
        }

        public Task<Response<BlobAppendInfo>> AppendBlockFromUriAsync(
            Uri sourceUri,
            HttpRange sourceRange = default,
            byte[] sourceContentHash = default,
            AppendBlobRequestConditions conditions = default,
            AppendBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.AppendBlockFromUriAsync(
                sourceUri, sourceRange, sourceContentHash,
                conditions, sourceConditions, cancellationToken);
        }

        public Response<BlobInfo> Seal(
            AppendBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.Seal(conditions, cancellationToken);
        }

        public Task<Response<BlobInfo>> SealAsync(
            AppendBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default)
        {
            return client.SealAsync(conditions, cancellationToken);
        }

        public Stream OpenWrite(
            bool overwrite,
            AppendBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenWrite(overwrite, options, cancellationToken);
        }

        public Task<Stream> OpenWriteAsync(
            bool overwrite,
            AppendBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default)
        {
            return client.OpenWriteAsync(overwrite, options, cancellationToken);
        }

        protected override IBlobBaseClient WithSnapshotOverride(string snapshot) => WithSnapshot(snapshot);

        protected override IBlobBaseClient WithVersionOverride(string versionId) => WithVersion(versionId);

        protected override IBlobBaseClient WithCustomerProvidedKeyOverride(CustomerProvidedKey? customerProvidedKey) =>
            WithCustomerProvidedKey(customerProvidedKey);

        protected override IBlobBaseClient WithEncryptionScopeOverride(string encryptionScope) =>
            WithEncryptionScope(encryptionScope);
    }
}