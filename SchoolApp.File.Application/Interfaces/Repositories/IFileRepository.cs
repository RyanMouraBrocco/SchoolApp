namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IFileRepository<TFile> where TFile : Application.Domain.Entities.File
{
    Task AddAsync(string folderPath, string fileName, Stream fileStream);
    Task<bool> ExistsAsync(string folderPath, string fileName);
    Task DeleteAsync(string folderPath, string fileName);
    IList<TFile> GetAllInPath(string folderPath);
}
