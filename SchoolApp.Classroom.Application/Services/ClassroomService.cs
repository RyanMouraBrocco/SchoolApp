using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Classroom.Application.Services;

public class ClassroomService : IClassroomService
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly IClassroomStudentRepository _classroomStudentRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;

    public ClassroomService(IClassroomRepository classroomRepository,
                            IClassroomStudentRepository classroomStudentRepository,
                            ISubjectRepository subjectRepository,
                            IStudentRepository studentRepository,
                            ITeacherRepository teacherRepository)
    {
        _classroomRepository = classroomRepository;
        _classroomStudentRepository = classroomStudentRepository;
        _subjectRepository = subjectRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<Domain.Entities.Classrooms.Classroom> CreateAsync(AuthenticatedUserObject requesterUser, Domain.Entities.Classrooms.Classroom newClassroom)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var subjectCheck = _subjectRepository.GetOneById(newClassroom.SubjectId);
        if (subjectCheck == null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        var teacherCheck = await _teacherRepository.GetOneByIdAsync(newClassroom.TeacherId);
        if (teacherCheck == null || teacherCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Teacher not found");

        newClassroom.AccountId = requesterUser.AccountId;
        newClassroom.CreationDate = DateTime.Now;
        newClassroom.CreatorId = requesterUser.UserId;
        newClassroom.UpdateDate = null;
        newClassroom.UpdaterId = null;

        var insertedClassroom = await _classroomRepository.InsertAsync(newClassroom);

        await UpdateStudentsArrayAsync(requesterUser.AccountId, insertedClassroom.Id, newClassroom.Students);

        return insertedClassroom;
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, int itemId)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var classroomCheck = _classroomRepository.GetOneById(itemId);
        if (classroomCheck == null || classroomCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Classroom not found");

        classroomCheck.UpdateDate = DateTime.Now;
        classroomCheck.UpdaterId = requesterUser.UserId;

        await _classroomRepository.UpdateAsync(classroomCheck);
        await _classroomRepository.DeleteAsync(itemId);
    }

    public IList<Domain.Entities.Classrooms.Classroom> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return requesterUser.Type switch
        {
            UserTypeEnum.Manager => _classroomRepository.GetAll(requesterUser.AccountId, top, skip),
            UserTypeEnum.Owner => _classroomRepository.GetAllByOwnerId(requesterUser.UserId, top, skip),
            UserTypeEnum.Teacher => _classroomRepository.GetAllByTeacherId(requesterUser.UserId, top, skip),
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public Domain.Entities.Classrooms.Classroom GetOneById(int id)
    {
        return _classroomRepository.GetOneById(id);
    }

    public async Task<Domain.Entities.Classrooms.Classroom> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, Domain.Entities.Classrooms.Classroom updatedClassroom)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        var classroomCheck = _classroomRepository.GetOneById(itemId);
        if (classroomCheck == null || classroomCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Classroom not found");

        var subjectCheck = _subjectRepository.GetOneById(updatedClassroom.SubjectId);
        if (subjectCheck == null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        var teacherCheck = await _teacherRepository.GetOneByIdAsync(updatedClassroom.TeacherId);
        if (teacherCheck == null || teacherCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Teacher not found");

        updatedClassroom.Id = itemId;
        updatedClassroom.AccountId = classroomCheck.AccountId;
        updatedClassroom.CreationDate = classroomCheck.CreationDate;
        updatedClassroom.CreatorId = classroomCheck.CreatorId;
        updatedClassroom.UpdateDate = DateTime.Now;
        updatedClassroom.UpdaterId = requesterUser.UserId;

        var resultClassroom = await _classroomRepository.UpdateAsync(updatedClassroom);

        await UpdateStudentsArrayAsync(requesterUser.AccountId, updatedClassroom.Id, updatedClassroom.Students);

        return resultClassroom;
    }

    private async Task UpdateStudentsArrayAsync(int accountId, int classroomId, IList<ClassroomStudent> students)
    {
        await _classroomStudentRepository.DeleteAllByClassroomIdAsync(classroomId);

        foreach (var student in students)
        {
            var studentCheck = _studentRepository.GetOneById(student.StudentId);
            if (studentCheck != null && studentCheck.AccountId == accountId)
            {
                student.ClassroomId = classroomId;
                await _classroomStudentRepository.InsertAsync(student);
            }
        }
    }
}
