using Azure.Storage.Blobs;

namespace OfflineDemoApi.Endpoints;

public static class FileEndpoints
{
    public static void AddFileEndpoints(this WebApplication app)
    {
        app.MapGet("/api/file", GetSecureFiles);
    }

    private static async Task<IResult> GetSecureFiles(IConfiguration config, string url)
    {
        var blobUri = new Uri(url);
        var storageAccountKey = config.GetValue<string>("AzureStorage:StorageAccountKey");
        var storageAccountName = config.GetValue<string>("AzureStorage:StorageAccountName");

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
