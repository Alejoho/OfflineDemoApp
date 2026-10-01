using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OfflineDemoApi.Data;
using OfflineDemoApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Bind Azure Storage configuration
var azureStorageConfig = builder.Configuration.GetSection("AzureStorage").Get<AzureStorageConfig>();

// Ensure the blob container exists
var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);
var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageConfig.ContainerName);
await blobContainerClient.CreateIfNotExistsAsync();
await blobContainerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

builder.Services.AddSingleton(blobContainerClient);

// Configure Kestrel limits
builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 524288000*2; // Example: 500 MB
	options.Limits.MaxRequestBufferSize = 524288000 * 2; // Example: 100 MB
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

app.MapGet("/", () => {
	return "Hello World";
});

app.MapGet("/api/shorts", async (ISqlDataAccess sql) =>
{
	return await sql.LoadData<ShortsModel, dynamic>("spShorts_GetAll", new { }, "sql");
});

app.MapGet("/api/file", async (IConfiguration config, string url) =>
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
});

app.MapPost("/api/upload", async (HttpRequest request,
								  BlobContainerClient containerClient,
								  ISqlDataAccess sql) =>
{
	if (!request.HasFormContentType || request.Form.Files.Count == 0)
	{
		return Results.BadRequest("Invalid form submission. Files are required.");
	}

	var form = await request.ReadFormAsync();

	// Extract form data
	var title = form["Title"].ToString();
	var description = form["Description"].ToString();
	var hashtags = form["Hashtags"].ToString();
	var mp4File = form.Files["Mp4File"];
	var imageFile = form.Files["ImageFile"];

	if (mp4File == null || imageFile == null)
	{
		return Results.BadRequest("Both MP4 and image files are required.");
	}

	// Validate file size
	const long maxFileSize = 800L * 1024 * 1024; // 800MB
	if (mp4File.Length > maxFileSize)
	{
		return Results.BadRequest("MP4 file exceeds the 800MB limit.");
	}

	var createNewParameters = new { title, description, hashtags };
	int id = await sql.SaveDataScalar<int, dynamic>("spShorts_CreateNew", createNewParameters, "sql");

	// Rename files
	string? mp4FileName = $"{id}.mp4";

	// TODO - Fix this extension lookup
	string? imageFileName = $"{id}.{imageFile.FileName.Split('.')[1]}";
	
	// Upload MP4 file
	var mp4BlobClient = containerClient.GetBlobClient($"shorts/{mp4FileName}");
	await using (var mp4Stream = mp4File.OpenReadStream())
	{
		await mp4BlobClient.UploadAsync(mp4Stream, true);
	}

	// Upload Image file
	var imageBlobClient = containerClient.GetBlobClient($"images/{imageFileName}");
	await using (var imageStream = imageFile.OpenReadStream())
	{
		await imageBlobClient.UploadAsync(imageStream, true);
	}

	// Update SQL with the uploaded files
	string? mp4FileUrl = mp4BlobClient.Uri.ToString();
	string? imageFileUrl = imageBlobClient.Uri.ToString();

	var addUploadedFilesParameters = new { id, mp4FileUrl, imageFileUrl };

	await sql.SaveData("spShorts_AddUploadedFiles", addUploadedFilesParameters, "sql");

	return Results.Ok(new { Message = "Files uploaded to Azure Blob Storage successfully." });
});

app.Run();