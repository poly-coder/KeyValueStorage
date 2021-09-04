using Azure.Storage.Blobs;
using DotNetX;
using DotNetX.Azure.Storage.Blobs;
using DotNetX.Azure.Storage.Blobs.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AzureStorageBlobsDIExtensions
    {
        public static IServiceCollection AddAzureStorageBlobsWrapperFactories(
            this IServiceCollection services)
        {
            services.TryAddSingleton<IBlobServiceClientWrapperFactory, BlobServiceClientWrapperFactory>();
            services.TryAddSingleton<IBlobContainerClientWrapperFactory, BlobContainerClientWrapperFactory>();
            services.TryAddSingleton<IBlobClientWrapperFactory, BlobClientWrapperFactory>();
            services.TryAddSingleton<IAppendBlobClientWrapperFactory, AppendBlobClientWrapperFactory>();
            services.TryAddSingleton<IBlockBlobClientWrapperFactory, BlockBlobClientWrapperFactory>();
            services.TryAddSingleton<IPageBlobClientWrapperFactory, PageBlobClientWrapperFactory>();
            services.TryAddSingleton<IPageableWrapperFactory, PageableWrapperFactory>();
            services.TryAddSingleton<IAsyncPageableWrapperFactory, AsyncPageableWrapperFactory>();

            return services;
        }

        #region [ CreateBlobServiceClient ]

        private static IBlobServiceClient CreateBlobServiceClient(
            this IServiceProvider provider,
            string connectionString)
        {
            var blobClient = new BlobServiceClient(connectionString);

            var factory = provider.GetRequiredService<IBlobServiceClientWrapperFactory>();

            var wrapper = factory.CreateWrapper(blobClient);

            return wrapper;
        }

        private static IBlobServiceClient CreateBlobServiceClient<TSettings>(
            this IServiceProvider provider,
            TSettings settings)
            where TSettings : IBlobServiceClientSettings =>
            provider.CreateBlobServiceClient(settings.ConnectionString);

        private static IBlobServiceClient CreateBlobServiceClient<TSettings>(
            this IServiceProvider provider)
            where TSettings : class, IBlobServiceClientSettings
        {
            var settings = provider.GetRequiredService<IOptions<TSettings>>().Value;

            return provider.CreateBlobServiceClient(settings);
        }

        private static IBlobServiceClient CreateNamedBlobServiceClient<TSettings>(
            this IServiceProvider provider,
            string optionsName)
            where TSettings : class, IBlobServiceClientSettings
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<TSettings>>().Get(optionsName);

            return provider.CreateBlobServiceClient(settings);
        }

        #endregion

        #region [ AddBlobServiceClient ]

        public static IServiceCollection AddBlobServiceClient<TSettings>(
            this IServiceCollection services)
            where TSettings : class, IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider => provider.CreateBlobServiceClient<TSettings>());

            return services;
        }

        public static IServiceCollection AddNamedBlobServiceClient<TSettings>(
            this IServiceCollection services,
            string optionName)
            where TSettings : class, IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider => provider.CreateNamedBlobServiceClient<TSettings>(optionName));

            return services;
        }

        public static IServiceCollection AddBlobServiceClient<TSettings>(
            this IServiceCollection services,
            TSettings settings)
            where TSettings : IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider => provider.CreateBlobServiceClient(settings));

            return services;
        }

        public static IServiceCollection AddBlobServiceClient(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider => provider.CreateBlobServiceClient(connectionString));

            return services;
        }

        #endregion

        #region [ AddBlobServiceClientProvider ]

        public static IServiceCollection AddBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            Func<IBlobServiceClient, TProvider> createProvider)
            where TSettings : class, IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobServiceClient<TSettings>()));

            return services;
        }

        public static IServiceCollection AddNamedBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            string optionName,
            Func<IBlobServiceClient, TProvider> createProvider)
            where TSettings : class, IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
                createProvider(provider.CreateNamedBlobServiceClient<TSettings>(optionName)));

            return services;
        }

        public static IServiceCollection AddBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            TSettings settings,
            Func<IBlobServiceClient, TProvider> createProvider)
            where TSettings : IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobServiceClient(settings)));

            return services;
        }

        public static IServiceCollection AddBlobServiceClientProvider<TProvider>(
            this IServiceCollection services,
            string connectionString,
            Func<IBlobServiceClient, TProvider> createProvider)
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobServiceClient(connectionString)));

            return services;
        }

        #endregion

        #region [ AddReliableBlobServiceClient ]

        public static IServiceCollection AddReliableBlobServiceClient<TSettings>(
            this IServiceCollection services,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                {
                    var service = provider.CreateBlobServiceClient<TSettings>();

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddNamedReliableBlobServiceClient<TSettings>(
            this IServiceCollection services,
            string optionName,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
            {
                return new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                {
                    var service = provider.CreateNamedBlobServiceClient<TSettings>(optionName);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobServiceClient<TSettings>(
            this IServiceCollection services,
            TSettings settings,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : IBlobServiceClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                {
                    var service = provider.CreateBlobServiceClient(settings);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobServiceClient(
            this IServiceCollection services,
            string connectionString,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton<ReliableAsyncService<IBlobServiceClient>>(provider =>
            {
                return new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                {
                    var service = provider.CreateBlobServiceClient(connectionString);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        #endregion

        #region [ AddReliableBlobServiceClientProvider ]

        public static IServiceCollection AddReliableBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            Func<ReliableAsyncService<IBlobServiceClient>, TProvider> createProvider,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                    {
                        var service = provider.CreateBlobServiceClient<TSettings>();

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddNamedReliableBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            string optionName,
            Func<ReliableAsyncService<IBlobServiceClient>, TProvider> createProvider,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                    {
                        var service = provider.CreateNamedBlobServiceClient<TSettings>(optionName);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobServiceClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            TSettings settings,
            Func<ReliableAsyncService<IBlobServiceClient>, TProvider> createProvider,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TSettings : IBlobServiceClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                    {
                        var service = provider.CreateBlobServiceClient(settings);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobServiceClientProvider<TProvider>(
            this IServiceCollection services,
            string connectionString,
            Func<ReliableAsyncService<IBlobServiceClient>, TProvider> createProvider,
            Func<IBlobServiceClient, CancellationToken, Task> onNewService)
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobServiceClient>(async token =>
                    {
                        var service = provider.CreateBlobServiceClient(connectionString);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        #endregion

        #region [ CreateBlobContainerClient ]

        private static IBlobContainerClient CreateBlobContainerClient(
            this IServiceProvider provider,
            string connectionString,
            string containerName)
        {
            var blobClient = new BlobServiceClient(connectionString);

            var containerClient = blobClient.GetBlobContainerClient(containerName);

            var factory = provider.GetRequiredService<IBlobContainerClientWrapperFactory>();

            var wrapper = factory.CreateWrapper(containerClient);

            return wrapper;
        }

        private static IBlobContainerClient CreateBlobContainerClient<TSettings>(
            this IServiceProvider provider,
            TSettings settings)
            where TSettings : IBlobContainerClientSettings =>
            provider.CreateBlobContainerClient(
                settings.ConnectionString,
                settings.Container);

        private static IBlobContainerClient CreateBlobContainerClient<TSettings>(
            this IServiceProvider provider)
            where TSettings : class, IBlobContainerClientSettings
        {
            var settings = provider.GetRequiredService<IOptions<TSettings>>().Value;

            return provider.CreateBlobContainerClient(settings);
        }

        private static IBlobContainerClient CreateNamedBlobContainerClient<TSettings>(
            this IServiceProvider provider,
            string optionsName)
            where TSettings : class, IBlobContainerClientSettings
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<TSettings>>().Get(optionsName);

            return provider.CreateBlobContainerClient(settings);
        }

        #endregion

        #region [ AddBlobContainerClient ]

        public static IServiceCollection AddBlobContainerClient<TSettings>(
            this IServiceCollection services)
            where TSettings : class, IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider => provider.CreateBlobContainerClient<TSettings>());

            return services;
        }

        public static IServiceCollection AddNamedBlobContainerClient<TSettings>(
            this IServiceCollection services,
            string optionName)
            where TSettings : class, IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider => provider.CreateNamedBlobContainerClient<TSettings>(optionName));

            return services;
        }

        public static IServiceCollection AddBlobContainerClient<TSettings>(
            this IServiceCollection services,
            TSettings settings)
            where TSettings : IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider => provider.CreateBlobContainerClient(settings));

            return services;
        }

        public static IServiceCollection AddBlobContainerClient(
            this IServiceCollection services,
            string connectionString,
            string containerName)
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                provider.CreateBlobContainerClient(connectionString, containerName));

            return services;
        }

        #endregion

        #region [ AddBlobContainerClientProvider ]

        public static IServiceCollection AddBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            Func<IBlobContainerClient, TProvider> createProvider)
            where TSettings : class, IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobContainerClient<TSettings>()));

            return services;
        }

        public static IServiceCollection AddNamedBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            string optionName,
            Func<IBlobContainerClient, TProvider> createProvider)
            where TSettings : class, IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
                createProvider(provider.CreateNamedBlobContainerClient<TSettings>(optionName)));

            return services;
        }

        public static IServiceCollection AddBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            TSettings settings,
            Func<IBlobContainerClient, TProvider> createProvider)
            where TSettings : IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobContainerClient(settings)));

            return services;
        }

        public static IServiceCollection AddBlobContainerClientProvider<TProvider>(
            this IServiceCollection services,
            string connectionString,
            string containerName,
            Func<IBlobContainerClient, TProvider> createProvider)
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
                createProvider(provider.CreateBlobContainerClient(connectionString, containerName)));

            return services;
        }

        #endregion

        #region [ AddReliableBlobContainerClient ]

        public static IServiceCollection AddReliableBlobContainerClient<TSettings>(
            this IServiceCollection services,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                {
                    var service = provider.CreateBlobContainerClient<TSettings>();

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddNamedReliableBlobContainerClient<TSettings>(
            this IServiceCollection services,
            string optionName,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
            {
                return new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                {
                    var service = provider.CreateNamedBlobContainerClient<TSettings>(optionName);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobContainerClient<TSettings>(
            this IServiceCollection services,
            TSettings settings,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : IBlobContainerClientSettings
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                {
                    var service = provider.CreateBlobContainerClient(settings);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobContainerClient(
            this IServiceCollection services,
            string connectionString,
            string containerName,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton<ReliableAsyncService<IBlobContainerClient>>(provider =>
            {
                return new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                {
                    var service = provider.CreateBlobContainerClient(connectionString, containerName);

                    await onNewService(service, token);

                    return service;
                });
            });

            return services;
        }

        #endregion

        #region [ AddReliableBlobContainerClientProvider ]

        public static IServiceCollection AddReliableBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            Func<ReliableAsyncService<IBlobContainerClient>, TProvider> createProvider,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                    {
                        var service = provider.CreateBlobContainerClient<TSettings>();

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddNamedReliableBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            string optionName,
            Func<ReliableAsyncService<IBlobContainerClient>, TProvider> createProvider,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : class, IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddScoped(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                    {
                        var service = provider.CreateNamedBlobContainerClient<TSettings>(optionName);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobContainerClientProvider<TSettings, TProvider>(
            this IServiceCollection services,
            TSettings settings,
            Func<ReliableAsyncService<IBlobContainerClient>, TProvider> createProvider,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TSettings : IBlobContainerClientSettings
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                    {
                        var service = provider.CreateBlobContainerClient(settings);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        public static IServiceCollection AddReliableBlobContainerClientProvider<TProvider>(
            this IServiceCollection services,
            string connectionString,
            string containerName,
            Func<ReliableAsyncService<IBlobContainerClient>, TProvider> createProvider,
            Func<IBlobContainerClient, CancellationToken, Task> onNewService)
            where TProvider : class
        {
            services.AddAzureStorageBlobsWrapperFactories();

            services.AddSingleton(provider =>
            {
                return createProvider(
                    new DelegateReliableAsyncService<IBlobContainerClient>(async token =>
                    {
                        var service = provider.CreateBlobContainerClient(connectionString, containerName);

                        await onNewService(service, token);

                        return service;
                    }));
            });

            return services;
        }

        #endregion
    }
}
