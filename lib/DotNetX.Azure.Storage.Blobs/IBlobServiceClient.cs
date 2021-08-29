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
    public interface IBlobServiceClient
    {
        Uri Uri { get; }

        string AccountName { get; }

        bool CanGenerateAccountSasUri { get; }

        IBlobContainerClient GetBlobContainerClient(
            string blobContainerName);

        IPageable<BlobContainerItem> GetBlobContainers(
            BlobContainerTraits traits = BlobContainerTraits.None,
            BlobContainerStates states = BlobContainerStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default);

        IAsyncPageable<BlobContainerItem> GetBlobContainersAsync(
            BlobContainerTraits traits = BlobContainerTraits.None,
            BlobContainerStates states = BlobContainerStates.None,
            string prefix = default,
            CancellationToken cancellationToken = default);

        Response<AccountInfo> GetAccountInfo(
            CancellationToken cancellationToken = default);

        Task<Response<AccountInfo>> GetAccountInfoAsync(
            CancellationToken cancellationToken = default);

        Response<BlobServiceProperties> GetProperties(
            CancellationToken cancellationToken = default);

        Task<Response<BlobServiceProperties>> GetPropertiesAsync(
            CancellationToken cancellationToken = default);

        Response SetProperties(
            BlobServiceProperties properties,
            CancellationToken cancellationToken = default);

        Task<Response> SetPropertiesAsync(
            BlobServiceProperties properties,
            CancellationToken cancellationToken = default);

        Response<BlobServiceStatistics> GetStatistics(
            CancellationToken cancellationToken = default);

        Task<Response<BlobServiceStatistics>> GetStatisticsAsync(
            CancellationToken cancellationToken = default);

        Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default);

        Task<Response<UserDelegationKey>> GetUserDelegationKeyAsync(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default);

        Response<BlobContainerClient> CreateBlobContainer(
            string blobContainerName,
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerClient>> CreateBlobContainerAsync(
            string blobContainerName,
            PublicAccessType publicAccessType = PublicAccessType.None,
            Metadata metadata = default,
            CancellationToken cancellationToken = default);

        Response DeleteBlobContainer(
            string blobContainerName,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Task<Response> DeleteBlobContainerAsync(
            string blobContainerName,
            BlobRequestConditions conditions = default,
            CancellationToken cancellationToken = default);

        Response<BlobContainerClient> UndeleteBlobContainer(
            string deletedContainerName,
            string deletedContainerVersion,
            CancellationToken cancellationToken = default);

        Task<Response<BlobContainerClient>> UndeleteBlobContainerAsync(
            string deletedContainerName,
            string deletedContainerVersion,
            CancellationToken cancellationToken = default);

        IPageable<TaggedBlobItem> FindBlobsByTags(
            string tagFilterSqlExpression,
            CancellationToken cancellationToken = default);

        IAsyncPageable<TaggedBlobItem> FindBlobsByTagsAsync(
            string tagFilterSqlExpression,
            CancellationToken cancellationToken = default);

        Uri GenerateAccountSasUri(
            AccountSasPermissions permissions,
            DateTimeOffset expiresOn,
            AccountSasResourceTypes resourceTypes);

        Uri GenerateAccountSasUri(AccountSasBuilder builder);
    }
}
