using System.Reflection.PortableExecutable;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Domain.Enums;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Classroom.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    private void CheckStudentFields(Student student)
    {
        if (!Enum.IsDefined(typeof(SexTypeEnum), student.Sex))
            throw new FormatException("This sex is not valid");

        if (string.IsNullOrEmpty(student.Name?.Trim()))
            throw new FormatException("Name can't be null or empty");
    }

    public async Task<Student> CreateAsync(AuthenticatedUserObject requesterUser, Student newStudent)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);
        CheckStudentFields(newStudent);

        newStudent.AccountId = requesterUser.AccountId;
        newStudent.CreationDate = DateTime.Now;
        newStudent.CreatorId = requesterUser.UserId;
        newStudent.UpdateDate = null;
        newStudent.UpdaterId = null;

        var insertedEntity = await _studentRepository.InsertAsync(newStudent);

        return insertedEntity;
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var studentCheck = _studentRepository.GetOneById(itemId);
        if (studentCheck == null || studentCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Student not found");

        studentCheck.UpdateDate = DateTime.Now;
        studentCheck.UpdaterId = requesterUser.UserId;

        await _studentRepository.UpdateAsync(studentCheck);
        await _studentRepository.DeleteAsync(itemId);
    }

    public IList<Student> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Owner => _studentRepository.GetAllByOwnerId(requesterUser.UserId, top, skip),
            UserTypeEnum.Teacher => _studentRepository.GetAllByTeacherId(requesterUser.UserId, top, skip),
            UserTypeEnum.Manager => _studentRepository.GetAll(requesterUser.AccountId, top, skip),
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public Student GetOneById(AuthenticatedUserObject requesterUser, int id)
    {
        if (requesterUser.Type == UserTypeEnum.Manager)
        {
            var studentCheck = _studentRepository.GetOneById(id);
            if (studentCheck == null || studentCheck.AccountId != requesterUser.AccountId)
                return null;

            return studentCheck;
        }
        else if (requesterUser.Type == UserTypeEnum.Teacher)
            return _studentRepository.GetOneByIdAndTeacherId(id, requesterUser.UserId);
        else if (requesterUser.Type == UserTypeEnum.Owner)
            return _studentRepository.GetOneByIdAndOwnerId(id, requesterUser.UserId);
        else
            throw new NotImplementedException("Invalid user type");
    }

    public IList<Student> GetAllByOwnerId(int ownerId, int top, int skip)
    {
        return _studentRepository.GetAllByOwnerId(ownerId, top, skip);
    }

    public IList<Student> GetAllByTeacherId(int teacherId, int top, int skip)
    {
        return _studentRepository.GetAllByTeacherId(teacherId, top, skip);
    }

    public async Task<Student> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, Student updatedStudent)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);
        CheckStudentFields(updatedStudent);

        var studentCheck = _studentRepository.GetOneById(itemId);
        if (studentCheck == null || studentCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Student not found");

        updatedStudent.Id = itemId;
        updatedStudent.AccountId = studentCheck.AccountId;
        updatedStudent.CreationDate = studentCheck.CreationDate;
        updatedStudent.CreatorId = studentCheck.CreatorId;
        updatedStudent.UpdateDate = DateTime.Now;
        updatedStudent.UpdaterId = requesterUser.UserId;

        return await _studentRepository.UpdateAsync(updatedStudent);
    }

    public Student GetOneById(int id)
    {
        return _studentRepository.GetOneById(id);
    }
}
