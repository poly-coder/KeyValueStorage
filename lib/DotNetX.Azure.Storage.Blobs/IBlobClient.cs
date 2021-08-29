using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public interface IBlobClient : IBlobBaseClient
    {
        new IBlobClient WithSnapshot(string snapshot);

        new IBlobClient WithVersion(string versionId);

        new IBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey);

        new IBlobClient WithEncryptionScope(string encryptionScope);

        Response<BlobContentInfo> Upload(Stream content);

        Response<BlobContentInfo> Upload(BinaryData content);

        Response<BlobContentInfo> Upload(string path);

        Task<Response<BlobContentInfo>> UploadAsync(Stream content);

        Task<Response<BlobContentInfo>> UploadAsync(BinaryData content);

        Task<Response<BlobContentInfo>> UploadAsync(string path);

        Response<BlobContentInfo> Upload(
            Stream content,
            CancellationToken cancellationToken);

        Response<BlobContentInfo> Upload(
            BinaryData content,
            CancellationToken cancellationToken);

        Response<BlobContentInfo> Upload(
            string path,
            CancellationToken cancellationToken);

        Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            CancellationToken cancellationToken);

        Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            CancellationToken cancellationToken);

        Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            CancellationToken cancellationToken);

        Response<BlobContentInfo> Upload(
            Stream content,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> Upload(
            BinaryData content,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> Upload(
            string path,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            bool overwrite = false,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> Upload(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> Upload(
            BinaryData content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Response<BlobContentInfo> Upload(
            string path,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default);
    }
}