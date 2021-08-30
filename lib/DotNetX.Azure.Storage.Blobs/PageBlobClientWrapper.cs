using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace DotNetX.Azure.Storage.Blobs
{
    public class PageBlobClientWrapper :
        BlobBaseClientWrapper,
        IPageBlobClient
    {
        private readonly PageBlobClient client;
        private readonly IPageBlobClientWrapperFactory pageBlobClientFactory;

        public PageBlobClientWrapper(
            PageBlobClient client,
            IPageBlobClientWrapperFactory pageBlobClientFactory) :
            base(client)
        {
            this.client = client;
            this.pageBlobClientFactory = pageBlobClientFactory;
        }

        protected override IBlobBaseClient WithSnapshotOverride(string snapshot) => WithSnapshot(snapshot);

        protected override IBlobBaseClient WithVersionOverride(string versionId) => WithVersion(versionId);

        protected override IBlobBaseClient WithCustomerProvidedKeyOverride(CustomerProvidedKey? customerProvidedKey) =>
            WithCustomerProvidedKey(customerProvidedKey);

        protected override IBlobBaseClient WithEncryptionScopeOverride(string encryptionScope) =>
            WithEncryptionScope(encryptionScope);

        public int PageBlobPageBytes => client.PageBlobPageBytes;
        public int PageBlobMaxUploadPagesBytes => client.PageBlobMaxUploadPagesBytes;

        public IPageBlobClient WithSnapshot(string snapshot) =>
            pageBlobClientFactory.CreateWrapper(client.WithSnapshot(snapshot));

        public IPageBlobClient WithVersion(string versionId) =>
            pageBlobClientFactory.CreateWrapper(client.WithVersion(versionId));

        public IPageBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey) =>
            pageBlobClientFactory.CreateWrapper(client.WithCustomerProvidedKey(customerProvidedKey));

        public IPageBlobClient WithEncryptionScope(string encryptionScope) =>
            pageBlobClientFactory.CreateWrapper(client.WithEncryptionScope(encryptionScope));

        public Response<BlobContentInfo> Create(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default) =>
            client.Create(size, options, cancellationToken);

        public Task<Response<BlobContentInfo>> CreateAsync(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default) =>
            client.CreateAsync(size, options, cancellationToken);

        public Response<BlobContentInfo> CreateIfNotExists(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default) =>
            client.CreateIfNotExists(size, options, cancellationToken);

        public Task<Response<BlobContentInfo>> CreateIfNotExistsAsync(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default) =>
            client.CreateIfNotExistsAsync(size, options, cancellationToken);

        public Response<PageInfo> UploadPages(
            Stream content,
            long offset,
            byte[] transactionalContentHash = default,
            PageBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default) =>
            client.UploadPages(
                content, offset, transactionalContentHash,
                conditions, progressHandler, cancellationToken);

        public Task<Response<PageInfo>> UploadPagesAsync(
            Stream content,
            long offset,
            byte[] transactionalContentHash = default,
            PageBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default) =>
            client.UploadPagesAsync(
                content, offset, transactionalContentHash,
                conditions, progressHandler, cancellationToken);

        public Response<PageInfo> ClearPages(
            HttpRange range,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.ClearPages(range, conditions, cancellationToken);

        public Task<Response<PageInfo>> ClearPagesAsync(
            HttpRange range,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.ClearPagesAsync(range, conditions, cancellationToken);

        public Response<PageRangesInfo> GetPageRanges(
            HttpRange? range = default,
            string snapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetPageRanges(range, snapshot, conditions, cancellationToken);

        public Task<Response<PageRangesInfo>> GetPageRangesAsync(
            HttpRange? range = default,
            string snapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetPageRangesAsync(range, snapshot, conditions, cancellationToken);

        public Response<PageRangesInfo> GetPageRangesDiff(
            HttpRange? range = default,
            string snapshot = default,
            string previousSnapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetPageRangesDiff(range, snapshot, previousSnapshot, conditions, cancellationToken);

        public Task<Response<PageRangesInfo>> GetPageRangesDiffAsync(
            HttpRange? range = default,
            string snapshot = default,
            string previousSnapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetPageRangesDiffAsync(
                range, previousSnapshot, previousSnapshot,
                conditions, cancellationToken);

        public Response<PageRangesInfo> GetManagedDiskPageRangesDiff(
            HttpRange? range = default,
            string snapshot = default,
            Uri previousSnapshotUri = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetManagedDiskPageRangesDiff(
                range, snapshot, previousSnapshotUri,
                conditions, cancellationToken);

        public Task<Response<PageRangesInfo>> GetManagedDiskPageRangesDiffAsync(
            HttpRange? range = default,
            string snapshot = default,
            Uri previousSnapshotUri = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.GetManagedDiskPageRangesDiffAsync(
                range, snapshot, previousSnapshotUri, 
                conditions, cancellationToken);

        public Response<PageBlobInfo> Resize(
            long size,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.Resize(size, conditions, cancellationToken);

        public Task<Response<PageBlobInfo>> ResizeAsync(
            long size,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.ResizeAsync(size, conditions, cancellationToken);

        public Response<PageBlobInfo> UpdateSequenceNumber(
            SequenceNumberAction action,
            long? sequenceNumber = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.UpdateSequenceNumber(action, sequenceNumber, conditions, cancellationToken);

        public Task<Response<PageBlobInfo>> UpdateSequenceNumberAsync(
            SequenceNumberAction action,
            long? sequenceNumber = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.UpdateSequenceNumberAsync(action, sequenceNumber, conditions, cancellationToken);

        public CopyFromUriOperation StartCopyIncremental(
            Uri sourceUri,
            string snapshot,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.StartCopyIncremental(sourceUri, snapshot, conditions, cancellationToken);

        public Task<CopyFromUriOperation> StartCopyIncrementalAsync(
            Uri sourceUri,
            string snapshot,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.StartCopyIncrementalAsync(sourceUri, snapshot, conditions, cancellationToken);

        public Response<PageInfo> UploadPagesFromUri(
            Uri sourceUri,
            HttpRange sourceRange,
            HttpRange range,
            byte[] sourceContentHash = default,
            PageBlobRequestConditions conditions = default,
            PageBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default) =>
            client.UploadPagesFromUri(
                sourceUri, sourceRange, sourceRange, 
                sourceContentHash, conditions,
                sourceConditions, cancellationToken);

        public Task<Response<PageInfo>> UploadPagesFromUriAsync(
            Uri sourceUri,
            HttpRange sourceRange,
            HttpRange range,
            byte[] sourceContentHash = default,
            PageBlobRequestConditions conditions = default,
            PageBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default) =>
            client.UploadPagesFromUriAsync(
                sourceUri, sourceRange, sourceRange, 
                sourceContentHash, conditions,
                sourceConditions, cancellationToken);

        public Stream OpenWrite(
            bool overwrite,
            long position,
            PageBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.OpenWrite(overwrite, position, options, cancellationToken);

        public Task<Stream> OpenWriteAsync(
            bool overwrite,
            long position,
            PageBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default) =>
            client.OpenWriteAsync(overwrite, position, options, cancellationToken);
    }
}