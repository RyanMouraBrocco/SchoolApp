using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Interfaces.Services;

public interface IFileService<TFile> where TFile : Application.Domain.Entities.File
{
    Task AddAsync(AuthenticatedUserObject requesterUser, TFile file);
    Task RemoveAsync(AuthenticatedUserObject requesterUser, string folderPath, TFile file);
}
