namespace OfflineDemoApi.Extensions;

public static class KestrelConfigurationExtensions
{
    public static void ConfigureKestrel(this WebApplicationBuilder builder)
    {
        // Configure Kestrel limits
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = Global.MaxFileSixe;
            options.Limits.MaxRequestBufferSize = Global.MaxFileSixe;
        });
    }
}