using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.File.Application.Services;

public class UserFileService : FileService<UserFile>, IUserFileService
{
    public UserFileService(IFileRepository<UserFile> fileRepository) : base(fileRepository)
    {
    }

    public Task AddAsync(AuthenticatedUserObject requesterUser, UserFile file)
    {
        return Task.Run(() => { Add(GetFolderFullPath(requesterUser), file); });
    }

    public IList<UserFile> GetAllInPath(AuthenticatedUserObject requesterUser)
    {
        return GetAllInPath(GetFolderFullPath(requesterUser));
    }
    public Task RemoveAsync(AuthenticatedUserObject requesterUser, string folderPath, UserFile file)
    {
        return Task.Run(() => { Remove(GetFolderFullPath(requesterUser), file); });
    }

    private string GetFolderFullPath(AuthenticatedUserObject requesterUser)
    {
        return $"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/";
    }

    private string GetTypePathByUserType(UserTypeEnum userType)
    {
        return userType switch
        {
            UserTypeEnum.Manager => "manager",
            UserTypeEnum.Owner => "onwer",
            UserTypeEnum.Teacher => "teacher",
            _ => throw new NotImplementedException("Invalid user type")
        };
    }
}