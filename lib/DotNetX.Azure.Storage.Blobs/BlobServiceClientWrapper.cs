using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.Threading;
using System.Threading.Tasks;
using Metadata = System.Collections.Generic.IDictionary<string, string>;

namespace DotNetX.Azure.Storage.Blobs
{
    public class BlobServiceClientWrapper : IBlobServiceClient
    {
        private readonly BlobServiceClient client;
        private readonly IBlobContainerClientWrapperFactory containerFactory;
        private readonly IPageableWrapperFactory pageableFactory;
        private readonly IAsyncPageableWrapperFactory asyncPageableFactory;

        public BlobServiceClientWrapper(
            BlobServiceClient client,
            IBlobContainerClientWrapperFactory containerFactory,
            IPageableWrapperFactory pageableFactory,
            IAsyncPageableWrapperFactory asyncPageableFactory)
        {
            this.client = client;
            this.containerFactory = containerFactory;
            this.pageableFactory = pageableFactory;
            this.asyncPageableFactory = asyncPageableFactory;
        }

        public Uri Uri { get; }
        public string AccountName { get; }
        public bool CanGenerateAccountSasUri { get; }

        public IBlobContainerClient GetBlobContainerClient(string blobContainerName) =>
            containerFactory.CreateWrapper(client.GetBlobContainerClient(blobContainerName));

        public IPageable<BlobContainerItem> GetBlobContainers(
            BlobContainerTraits traits = BlobContainerTraits.None,
            BlobContainerStates states = BlobContainerStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            pageableFactory.CreateWrapper(
                client.GetBlobContainers(
                    traits, states, prefix, cancellationToken));

        public IAsyncPageable<BlobContainerItem> GetBlobContainersAsync(
            BlobContainerTraits traits = BlobContainerTraits.None,
            BlobContainerStates states = BlobContainerStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default) =>
            asyncPageableFactory.CreateWrapper(
                client.GetBlobContainersAsync(
                    traits, states, prefix, cancellationToken));

        public Response<AccountInfo> GetAccountInfo(CancellationToken cancellationToken = default) =>
            client.GetAccountInfo(cancellationToken);

        public Task<Response<AccountInfo>> GetAccountInfoAsync(CancellationToken cancellationToken = default) =>
            client.GetAccountInfoAsync(cancellationToken);

        public Response<BlobServiceProperties> GetProperties(CancellationToken cancellationToken = default) =>
            client.GetProperties(cancellationToken);

        public Task<Response<BlobServiceProperties>> GetPropertiesAsync(
            CancellationToken cancellationToken = default) =>
            client.GetPropertiesAsync(cancellationToken);

        public Response SetProperties(
            BlobServiceProperties properties,
            CancellationToken cancellationToken = default) =>
            client.SetProperties(properties, cancellationToken);

        public Task<Response> SetPropertiesAsync(
            BlobServiceProperties properties,
            CancellationToken cancellationToken = default) =>
            client.SetPropertiesAsync(properties, cancellationToken);

        public Response<BlobServiceStatistics> GetStatistics(
            CancellationToken cancellationToken = default) =>
            client.GetStatistics(cancellationToken);

        public Task<Response<BlobServiceStatistics>> GetStatisticsAsync(
            CancellationToken cancellationToken = default) =>
            client.GetStatisticsAsync(cancellationToken);

        public Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default) =>
            client.GetUserDelegationKey(startsOn, expiresOn, cancellationToken);

        public Task<Response<UserDelegationKey>> GetUserDelegationKeyAsync(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default) =>
            client.GetUserDelegationKeyAsync(startsOn, expiresOn, cancellationToken);

        public Response<BlobContainerClient> CreateBlobContainer(
            string blobContainerName,
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            CancellationToken cancellationToken = default) =>
            client.CreateBlobContainer(blobContainerName, publicAccessType, metadata, cancellationToken);

        public Task<Response<BlobContainerClient>> CreateBlobContainerAsync(
            string blobContainerName,
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            CancellationToken cancellationToken = default) =>
            client.CreateBlobContainerAsync(
                blobContainerName, publicAccessType, metadata, cancellationToken);

        public Response DeleteBlobContainer(
            string blobContainerName,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlobContainer(blobContainerName, conditions, cancellationToken);

        public Task<Response> DeleteBlobContainerAsync(
            string blobContainerName,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default) =>
            client.DeleteBlobContainerAsync(blobContainerName, conditions, cancellationToken);

        public Response<BlobContainerClient> UndeleteBlobContainer(
            string deletedContainerName,
            string deletedContainerVersion,
            CancellationToken cancellationToken = default) =>
            client.UndeleteBlobContainer(
                deletedContainerName, deletedContainerVersion, cancellationToken);

        public Task<Response<BlobContainerClient>> UndeleteBlobContainerAsync(
            string deletedContainerName,
            string deletedContainerVersion,
            CancellationToken cancellationToken = default) =>
            client.UndeleteBlobContainerAsync(
                deletedContainerName, deletedContainerVersion, cancellationToken);

        public IPageable<TaggedBlobItem> FindBlobsByTags(
            string tagFilterSqlExpression,
            CancellationToken cancellationToken = default) =>
            pageableFactory.CreateWrapper(
                client.FindBlobsByTags(
                    tagFilterSqlExpression, cancellationToken));

        public IAsyncPageable<TaggedBlobItem> FindBlobsByTagsAsync(
            string tagFilterSqlExpression,
            CancellationToken cancellationToken = default) =>
            asyncPageableFactory.CreateWrapper(
                client.FindBlobsByTagsAsync(
                    tagFilterSqlExpression, cancellationToken));

        public Uri GenerateAccountSasUri(
            AccountSasPermissions permissions,
            DateTimeOffset expiresOn,
            AccountSasResourceTypes resourceTypes) =>
            client.GenerateAccountSasUri(permissions, expiresOn, resourceTypes);

        public Uri GenerateAccountSasUri(AccountSasBuilder builder) =>
            client.GenerateAccountSasUri(builder);
    }
}