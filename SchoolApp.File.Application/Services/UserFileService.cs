using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.File.Application.Services;

public class UserFileService : FileService<UserFile>
{
    public UserFileService(IFileRepository<UserFile> fileRepository) : base(fileRepository)
    {
    }

    public void Add(AuthenticatedUserObject requesterUser, UserFile file)
    {
        Add(GetFolderFullPath(requesterUser), file);
    }

    public IList<UserFile> GetAllInPath(AuthenticatedUserObject requesterUser)
    {
        return GetAllInPath(GetFolderFullPath(requesterUser));
    }
    public void Remove(AuthenticatedUserObject requesterUser, string folderPath, UserFile file)
    {
        Remove(GetFolderFullPath(requesterUser), file);
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
