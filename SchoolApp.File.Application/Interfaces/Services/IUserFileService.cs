using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Interfaces.Services;

public interface IUserFileService : IFileService<UserFile>
{
    IList<UserFile> GetAll(AuthenticatedUserObject requesterUser);
}
