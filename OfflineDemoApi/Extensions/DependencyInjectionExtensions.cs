using Azure.Storage.Blobs;
using OfflineDemoApi.Data;
using OfflineDemoApi.Models;

namespace OfflineDemoApi.Extensions;

public static class DependencyInjectionExtensions
{
    private static async Task AddAzureBlobStorageAsync(this WebApplicationBuilder builder)
    {
        // Bind Azure Storage configuration
        var azureStorageConfig = builder.Configuration.GetSection("AzureStorage").Get<AzureStorageConfig>();

        // Ensure the blob container exists
        var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageConfig.ContainerName);
        await blobContainerClient.CreateIfNotExistsAsync();
        await blobContainerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

        builder.Services.AddSingleton(blobContainerClient);
    }

    public static async Task AddDependenciesAsync(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices();

        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

        builder.Services.AddCorsServices();

        await builder.AddAzureBlobStorageAsync();
    }
}