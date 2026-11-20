using System.ComponentModel.DataAnnotations;

namespace OfflineDemoApi.Models;

public class AzureStorageConfig
{
    public const string SectionName = "AzureStorage";

    [Required]
    public string? ConnectionString { get; set; }

    [Required]
    public string? ContainerName { get; set; }

    [Required]
    public string? StorageAccountName { get; set; }

    [Required]
    public string? StorageAccountKey { get; set; }
}
