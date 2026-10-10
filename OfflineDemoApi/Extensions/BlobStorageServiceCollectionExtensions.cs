using Azure.Storage.Blobs;
using OfflineDemoApi.Models;

namespace OfflineDemoApi.Extensions;

public static class BlobStorageServiceCollectionExtensions
{
    public static async Task<IServiceCollection> AddAzureBlobStorageAsync(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var azureStorageConfig = configuration.GetSection("AzureStorage").Get<AzureStorageOptions>();

        var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageConfig.ContainerName);
        await blobContainerClient.CreateIfNotExistsAsync();
        await blobContainerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

        services.AddSingleton(blobContainerClient);
        return services;
    }
}