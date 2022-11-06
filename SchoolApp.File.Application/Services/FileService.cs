using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Services;

public abstract class FileService<TFile> where TFile : Application.Domain.Entities.File
{
    private readonly IFileRepository<TFile> _fileRepository;

    public FileService(IFileRepository<TFile> fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task AddAsync(string folderPath, TFile file)
    {
        if (string.IsNullOrEmpty(file.Base64Value?.Trim()))
            throw new FormatException("Base64Value can't be null or empty");

        if (string.IsNullOrEmpty(file.Extension?.Trim()))
            throw new FormatException("Extension can't be null or empty");

        file.FileName = Guid.NewGuid().ToString("N");

        await _fileRepository.AddAsync(folderPath, file.FileName, Base64ToStream(file.Base64Value));
    }

    public async Task RemoveAsync(string folderPath, TFile file)
    {
        if (string.IsNullOrEmpty(file.FileName?.ToString().Trim()))
            throw new FormatException("FileName can't not be null or empty");

        if (!await _fileRepository.ExistsAsync(folderPath, file.FileName))
            throw new UnauthorizedAccessException("File not found");

        await _fileRepository.DeleteAsync(folderPath, file.FileName);
    }

    public IList<TFile> GetAllInPath(string folderPath)
    {
        return _fileRepository.GetAllInPath(folderPath);
    }

    public MemoryStream Base64ToStream(string base64String)
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        return new MemoryStream(fileBytes, 0, fileBytes.Length);
    }
}
