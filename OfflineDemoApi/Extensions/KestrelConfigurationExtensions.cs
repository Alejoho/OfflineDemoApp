namespace OfflineDemoApi.Extensions;

public static class KestrelConfigurationExtensions
{
    public static void ConfigureKestrel(this WebApplicationBuilder builder)
    {
        // Configure Kestrel limits
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = 524288000 * 2; // Example: 500 MB
            options.Limits.MaxRequestBufferSize = 524288000 * 2; // Example: 100 MB
        });
    }
}