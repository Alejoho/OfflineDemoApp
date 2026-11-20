namespace OfflineDemoApi.Models;

public class AzureStorageConfig
{
    public const string SectionName = "AzureStorage";
    public string? ConnectionString { get; set; }
    public string? ContainerName { get; set; }
    public string? StorageAccountName { get; set; }
    public string? StorageAccountKey { get; set; }
}
