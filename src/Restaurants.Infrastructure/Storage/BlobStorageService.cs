using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Helpers;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> options) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = options.Value;

    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        string blobUrl = "";
        try
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            string contentType=FileHelper.GetMimeType(fileName);
            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            await blobClient.UploadAsync(data, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
            blobUrl = blobClient.Uri.ToString();
            return blobUrl;
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine($"Error Code: {ex.ErrorCode}");
            Console.WriteLine($"Error Message: {ex.Message}");

            if (ex.ErrorCode == "ContainerNotFound")
            {
                Console.WriteLine("The container does not exist.");
            }
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }
    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null) return null;
        
        var sasBuilder=new BlobSasBuilder()
        {
            BlobContainerName=_blobStorageSettings.LogosContainerName,
            Resource="b",
            StartsOn=DateTimeOffset.UtcNow.AddMinutes(-5),
            ExpiresOn=DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName= GetBlobNameFromUrl(blobUrl)

        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

        var sasToken = sasBuilder.ToSasQueryParameters(
                new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSettings.AccountKey))
                .ToString();
        return $"{blobUrl}?{sasToken}";
        //blob:https://tamlvsadev.blob.core.windows.net/logos/images.png
        //sas: sp=r&st=2025-01-16T03:47:18Z&se=2025-01-17T11:47:18Z&spr=https&sv=2022-11-02&sr=b&sig=QTkNBejWCw6Y8HAqeyKB5%2FHPUbvy28a3FYJelZfjhFc%3D
    }

    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri=new Uri(blobUrl);
        return uri.Segments.Last();
    }
}