using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IPageBlobClient : IBlobBaseClient
    {
        int PageBlobPageBytes { get; }

        int PageBlobMaxUploadPagesBytes { get; }

        new IPageBlobClient WithSnapshot(string snapshot);

        new IPageBlobClient WithVersion(string versionId);

        new IPageBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey);

        new IPageBlobClient WithEncryptionScope(string encryptionScope);

        Response<BlobContentInfo> Create(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> CreateAsync(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> CreateIfNotExists(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> CreateIfNotExistsAsync(
            long size,
            PageBlobCreateOptions options,
            CancellationToken cancellationToken = default);

        Response<PageInfo> UploadPages(
            Stream content,
            long offset,
            byte[] transactionalContentHash = default,
            PageBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageInfo>> UploadPagesAsync(
            Stream content,
            long offset,
            byte[] transactionalContentHash = default,
            PageBlobRequestConditions conditions = default,
            IProgress<long> progressHandler = default,
            CancellationToken cancellationToken = default);

        Response<PageInfo> ClearPages(
            HttpRange range,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageInfo>> ClearPagesAsync(
            HttpRange range,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageRangesInfo> GetPageRanges(
            HttpRange? range = default,
            string snapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageRangesInfo>> GetPageRangesAsync(
            HttpRange? range = default,
            string snapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageRangesInfo> GetPageRangesDiff(
            HttpRange? range = default,
            string snapshot = default,
            string previousSnapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageRangesInfo>> GetPageRangesDiffAsync(
            HttpRange? range = default,
            string snapshot = default,
            string previousSnapshot = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageRangesInfo> GetManagedDiskPageRangesDiff(
            HttpRange? range = default,
            string snapshot = default,
            Uri previousSnapshotUri = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageRangesInfo>> GetManagedDiskPageRangesDiffAsync(
            HttpRange? range = default,
            string snapshot = default,
            Uri previousSnapshotUri = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageBlobInfo> Resize(
            long size,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageBlobInfo>> ResizeAsync(
            long size,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageBlobInfo> UpdateSequenceNumber(
            SequenceNumberAction action,
            long? sequenceNumber = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageBlobInfo>> UpdateSequenceNumberAsync(
            SequenceNumberAction action,
            long? sequenceNumber = default,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        CopyFromUriOperation StartCopyIncremental(
            Uri sourceUri,
            string snapshot,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<CopyFromUriOperation> StartCopyIncrementalAsync(
            Uri sourceUri,
            string snapshot,
            PageBlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<PageInfo> UploadPagesFromUri(
            Uri sourceUri,
            HttpRange sourceRange,
            HttpRange range,
            byte[] sourceContentHash = default,
            PageBlobRequestConditions conditions = default,
            PageBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default);

        Task<Response<PageInfo>> UploadPagesFromUriAsync(
            Uri sourceUri,
            HttpRange sourceRange,
            HttpRange range,
            byte[] sourceContentHash = default,
            PageBlobRequestConditions conditions = default,
            PageBlobRequestConditions sourceConditions = default,
            CancellationToken cancellationToken = default);

        Stream OpenWrite(
            bool overwrite,
            long position,
            PageBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);

        Task<Stream> OpenWriteAsync(
            bool overwrite,
            long position,
            PageBlobOpenWriteOptions options = default,
            CancellationToken cancellationToken = default);
    }
}