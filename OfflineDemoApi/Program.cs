using OfflineDemoApi.Endpoints;
using OfflineDemoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

await builder.AddDependenciesAsync();

builder.ConfigureKestrel();

var app = builder.Build();

app.ApplyCorsDefaultPolicy();

app.ConfigureOpenApi();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.AddFileEndpoints();

app.AddShortEndpoints();

app.Run();