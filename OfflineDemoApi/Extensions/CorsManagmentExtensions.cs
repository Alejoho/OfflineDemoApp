namespace OfflineDemoApi.Extensions;

public static class CorsManagmentExtensions
{
    public static void AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }

    public static void ApplyCorsDefaultPolicy(this WebApplication app)
    {
        app.UseCors("AllowAnyOrigin");
    }
}