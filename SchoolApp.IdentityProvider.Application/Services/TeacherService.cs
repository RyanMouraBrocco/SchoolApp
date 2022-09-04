using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Enums;
using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Application.Helpers;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Validations;

namespace SchoolApp.IdentityProvider.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public IList<Teacher> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => _teacherRepository.GetAll(requesterUser.AccountId, top, skip),
            UserTypeEnum.Teacher => new List<Teacher>() { _teacherRepository.GetOneById(requesterUser.UserId) },
            UserTypeEnum.Owner => throw new NotImplementedException(),
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public async Task<Teacher> CreateAsync(AuthenticatedUserObject requesterUser, Teacher newTeacher)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);
        UserValidation.IsSecurityPassword(newTeacher.Password);
        GenericValidation.IsNotNegativeValue(nameof(newTeacher.Salary), newTeacher.Salary);

        var duplicatedEmail = _teacherRepository.GetOneByEmail(newTeacher.Email);
        if (duplicatedEmail != null)
            throw new UnauthorizedAccessException("This email has already used");

        newTeacher.Password = Utils.HashText(newTeacher.Password);
        newTeacher.AccountId = requesterUser.AccountId;
        newTeacher.CreationDate = DateTime.Now;
        newTeacher.CreatorId = requesterUser.UserId;
        newTeacher.UpdaterId = null;
        newTeacher.UpdateDate = null;

        return await _teacherRepository.InsertAsync(newTeacher);
    }

    public async Task<Teacher> UpdateAsync(AuthenticatedUserObject requesterUser, int teacherId, Teacher updatedTeacher)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);
        GenericValidation.IsNotNegativeValue(nameof(updatedTeacher.Salary), updatedTeacher.Salary);

        var teacherCheck = _teacherRepository.GetOneById(teacherId);
        if (teacherCheck == null || teacherCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Teacher not found");

        var duplicatedEmail = _teacherRepository.GetOneByEmail((string)updatedTeacher.Email);
        if (duplicatedEmail != null && duplicatedEmail.Id != teacherId)
            throw new UnauthorizedAccessException("This email has already used");

        updatedTeacher.Id = teacherId;
        updatedTeacher.Password = teacherCheck.Password;
        updatedTeacher.AccountId = requesterUser.AccountId;
        updatedTeacher.CreationDate = teacherCheck.CreationDate;
        updatedTeacher.CreatorId = teacherCheck.CreatorId;
        updatedTeacher.UpdaterId = requesterUser.UserId;
        updatedTeacher.UpdateDate = DateTime.Now;

        return await _teacherRepository.UpdateAsync(updatedTeacher);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int teacherId)
    {
        UserValidation.CheckOnlyManagerUser(requesterUser.Type);

        var ownerCheck = _teacherRepository.GetOneById(teacherId);
        if (ownerCheck == null || ownerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Teacher not found");

        ownerCheck.Id = teacherId;
        ownerCheck.UpdaterId = requesterUser.UserId;
        ownerCheck.UpdateDate = DateTime.Now;

        await _teacherRepository.UpdateAsync(ownerCheck);
        await _teacherRepository.DeleteAsync(teacherId);
    }
}
