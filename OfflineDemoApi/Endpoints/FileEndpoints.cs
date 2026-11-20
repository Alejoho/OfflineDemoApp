using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using OfflineDemoApi.Models;

namespace OfflineDemoApi.Endpoints;

public static class FileEndpoints
{
    public static void AddFileEndpoints(this WebApplication app)
    {
        app.MapGet("/api/file", GetSecureFiles);
    }

    private static async Task<IResult> GetSecureFiles(IOptions<AzureStorageConfig> azureConfig, string url)
    {
        var azureConfigValue = azureConfig.Value;

        var blobUri = new Uri(url);
        var storageAccountKey = azureConfigValue.StorageAccountKey;
        var storageAccountName = azureConfigValue.StorageAccountName;

        var credential = new Azure.Storage.StorageSharedKeyCredential(storageAccountName, storageAccountKey);
        var blobClient = new BlobClient(blobUri, credential);

        var downloadResponse = await blobClient.DownloadStreamingAsync();
        return Results.File(
                downloadResponse.Value.Content,
                downloadResponse.Value.Details.ContentType,
                fileDownloadName: blobUri.Segments.Last()
            );
    }
}
