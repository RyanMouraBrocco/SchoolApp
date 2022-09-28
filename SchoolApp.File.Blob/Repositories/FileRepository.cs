using System.IO.Compression;
using System.Data.SqlTypes;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Blob.Settings;

namespace SchoolApp.File.Blob.Repositories;

public class FileRepository<TFile> : IFileRepository<TFile> where TFile : Application.Domain.Entities.File, new()
{
    public AzureBlobStorageSettings Settings { get; set; }
    private readonly BlobContainerClient _container;

    public FileRepository(IOptions<AzureBlobStorageSettings> options)
    {
        Settings = options.Value;
        _container = new BlobContainerClient(Settings.ConnectionString, Settings.ContainerName);
        _container.CreateIfNotExists();
    }

    public async Task AddAsync(string folderPath, string fileName, Stream fileStream)
    {
        var blobClient = _container.GetBlobClient(folderPath + fileName);
        await blobClient.UploadAsync(fileStream);
    }

    public async Task<bool> ExistsAsync(string folderPath, string fileName)
    {
        var blobClient = _container.GetBlobClient(folderPath + fileName);
        return await blobClient.ExistsAsync();
    }

    public async Task DeleteAsync(string folderPath, string fileName)
    {
        var blobClient = _container.GetBlobClient(folderPath + fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public IList<TFile> GetAllInPath(string folderPath)
    {
        var blobs = _container.GetBlobs(prefix: folderPath);
        var result = new List<TFile>();

        foreach (var blob in blobs)
        {
            var blobClient = _container.GetBlobClient(blob.Name);
            var fileStream = new MemoryStream();
            blobClient.DownloadTo(fileStream);
            var fileBytes = fileStream.ToArray();
            result.Add(new TFile()
            {
                Base64Value = Convert.ToBase64String(fileBytes),
                FileName = blob.Name.Replace(folderPath, "")
            });
        }

        return result;
    }

}
