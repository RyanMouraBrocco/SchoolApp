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

    public void Add(string folderPath, TFile file)
    {
        if (string.IsNullOrEmpty(file.Base64Value?.Trim()))
            throw new FormatException("Base64Value can't be null or empty");

        if (string.IsNullOrEmpty(file.Extension?.Trim()))
            throw new FormatException("Extension can't be null or empty");

        file.FileName = Guid.NewGuid().ToString("N");

        _fileRepository.Add(folderPath, file.FileName, Base64ToStream(file.Base64Value));
    }

    public void Remove(string folderPath, TFile file)
    {
        if (string.IsNullOrEmpty(file.FileName?.ToString()))
            throw new FormatException("FileName can't not be null or empty");

        if (!_fileRepository.Exists(folderPath, file.FileName))
            throw new UnauthorizedAccessException("File not found");

        _fileRepository.Delete(folderPath, file.FileName);
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
