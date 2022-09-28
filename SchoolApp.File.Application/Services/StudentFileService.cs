using System.Security.Cryptography.X509Certificates;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.File.Application.Services;

public class StudentFileService : FileService<StudentFile>, IStudentFileService
{
    private readonly IStudentRepository _studentRepository;

    public StudentFileService(IFileRepository<StudentFile> fileRepository, IStudentRepository studentRepository) : base(fileRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task AddAsync(AuthenticatedUserObject requesterUser, StudentFile file)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);
        await ValidadeStudent(requesterUser.UserId, file.StudentId);
        await AddAsync(GetFolderFullPath(file.StudentId), file);
    }

    public async Task RemoveAsync(AuthenticatedUserObject requesterUser, StudentFile file)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);
        await ValidadeStudent(requesterUser.UserId, file.StudentId);
        await RemoveAsync(GetFolderFullPath(file.StudentId), file);
    }

    public async Task<IList<StudentFile>> GetAllByStudentIdAsync(AuthenticatedUserObject requesterUser, int studentId)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);
        await ValidadeStudent(requesterUser.UserId, studentId);
        return GetAllInPath(GetFolderFullPath(studentId));
    }

    private async Task ValidadeStudent(int ownerId, int studentId)
    {
        var allStudents = await _studentRepository.GetAllByOwnerIdAsync(ownerId);
        if (!allStudents.Select(x => x.Id).Contains(studentId))
            throw new UnauthorizedAccessException("Student not found");
    }

    private string GetFolderFullPath(int studentId)
    {
        return $"students/{studentId}/";
    }
}
