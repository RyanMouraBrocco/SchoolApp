namespace SchoolApp.File.Application.Interfaces.Repositories;

public interface IFileRepository<TFile> where TFile : Application.Domain.Entities.File
{
    void Add(string folderPath, string fileName, Stream fileStream);
    bool Exists(string folderPath, string fileName);
    void Delete(string folderPath, string fileName);
    IList<TFile> GetAllInPath(string folderPath);
}
