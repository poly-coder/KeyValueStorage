using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobClientWrapper :
        BlobBaseClientWrapper,
        IBlobClient
    {
        private readonly BlobClient client;
        private readonly IBlobClientWrapperFactory blobClientFactory;

        public BlobClientWrapper(
            BlobClient client,
            IBlobClientWrapperFactory blobClientFactory) :
            base(client)
        {
            this.client = client;
            this.blobClientFactory = blobClientFactory;
        }

        protected override IBlobBaseClient WithSnapshotOverride(string snapshot) => WithSnapshot(snapshot);

        protected override IBlobBaseClient WithVersionOverride(string versionId) => WithVersion(versionId);

        protected override IBlobBaseClient WithCustomerProvidedKeyOverride(CustomerProvidedKey? customerProvidedKey) =>
            WithCustomerProvidedKey(customerProvidedKey);

        protected override IBlobBaseClient WithEncryptionScopeOverride(string encryptionScope) =>
            WithEncryptionScope(encryptionScope);

        public IBlobClient WithSnapshot(string snapshot) =>
            blobClientFactory.CreateWrapper(client.WithSnapshot(snapshot));

        public IBlobClient WithVersion(string versionId) =>
            blobClientFactory.CreateWrapper(client.WithVersion(versionId));

        public IBlobClient WithCustomerProvidedKey(CustomerProvidedKey? customerProvidedKey) =>
            blobClientFactory.CreateWrapper(client.WithCustomerProvidedKey(customerProvidedKey));

        public IBlobClient WithEncryptionScope(string encryptionScope) =>
            blobClientFactory.CreateWrapper(client.WithEncryptionScope(encryptionScope));

        public Response<BlobContentInfo> Upload(Stream content) =>
            client.Upload(content);

        public Response<BlobContentInfo> Upload(BinaryData content) =>
            client.Upload(content);

        public Response<BlobContentInfo> Upload(string path) =>
            client.Upload(path);

        public Task<Response<BlobContentInfo>> UploadAsync(Stream content) =>
            client.UploadAsync(content);

        public Task<Response<BlobContentInfo>> UploadAsync(BinaryData content) =>
            client.UploadAsync(content);

        public Task<Response<BlobContentInfo>> UploadAsync(string path) =>
            client.UploadAsync(path);

        public Response<BlobContentInfo> Upload(
            Stream content,
            CancellationToken cancellationToken) =>
            client.Upload(content, cancellationToken);

        public Response<BlobContentInfo> Upload(
            BinaryData content,
            CancellationToken cancellationToken) =>
            client.Upload(content, cancellationToken);

        public Response<BlobContentInfo> Upload(
            string path,
            CancellationToken cancellationToken) =>
            client.Upload(path, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            CancellationToken cancellationToken) =>
            client.UploadAsync(content, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            CancellationToken cancellationToken) =>
            client.UploadAsync(content, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            CancellationToken cancellationToken) =>
            client.UploadAsync(path, cancellationToken);

        public Response<BlobContentInfo> Upload(
            Stream content,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.Upload(content, overwrite, cancellationToken);

        public Response<BlobContentInfo> Upload(
            BinaryData content,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.Upload(content, overwrite, cancellationToken);

        public Response<BlobContentInfo> Upload(
            string path,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.Upload(path, overwrite, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(content, overwrite, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(content, overwrite, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            bool overwrite = false,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(path, overwrite, cancellationToken);

        public Response<BlobContentInfo> Upload(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.Upload(content, options, cancellationToken);

        public Response<BlobContentInfo> Upload(
            BinaryData content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.Upload(content, options, cancellationToken);

        public Response<BlobContentInfo> Upload(
            string path,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.Upload(path, options, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            Stream content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(content, options, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            BinaryData content,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(content, options, cancellationToken);

        public Task<Response<BlobContentInfo>> UploadAsync(
            string path,
            BlobUploadOptions options,
            CancellationToken cancellationToken = default) =>
            client.UploadAsync(path, options, cancellationToken);
    }
}
