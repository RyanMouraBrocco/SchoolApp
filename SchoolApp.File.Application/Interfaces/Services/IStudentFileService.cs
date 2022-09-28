using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Interfaces.Services;

public interface IStudentFileService : IFileService<StudentFile>
{
    Task<IList<StudentFile>> GetAllByStudentIdAsync(AuthenticatedUserObject requesterUser, int studentId);
}
