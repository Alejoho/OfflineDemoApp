using OfflineDemoApi.Data;
using OfflineDemoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

await builder.Services.AddAzureBlobStorageAsync(builder.Configuration);

// Configure Kestrel limits
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1000L * 1024 * 1024; // Example: 500 MB
    options.Limits.MaxRequestBufferSize = 1000L * 1024 * 1024; // Example: 100 MB
});

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGroup("/api").MapApiEndpoints();

app.Run();