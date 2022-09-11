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

    public ClassroomService(IClassroomRepository classroomRepository,
                            IClassroomStudentRepository classroomStudentRepository,
                            ISubjectRepository subjectRepository)
    {
        _classroomRepository = classroomRepository;
        _classroomStudentRepository = classroomStudentRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<Domain.Entities.Classrooms.Classroom> CreateAsync(AuthenticatedUserObject requesterUser, Domain.Entities.Classrooms.Classroom newClassroom)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        // TODO: Validade teacher id with service api

        var subjectCheck = _subjectRepository.GetOneById(newClassroom.SubjectId);
        if (subjectCheck == null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        newClassroom.AccountId = requesterUser.AccountId;
        newClassroom.CreationDate = DateTime.Now;
        newClassroom.CreatorId = requesterUser.UserId;
        newClassroom.UpdateDate = null;
        newClassroom.UpdaterId = null;

        var insertedClassroom = await _classroomRepository.InsertAsync(newClassroom);

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
            UserTypeEnum.Manager => new List<Domain.Entities.Classrooms.Classroom>(),
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    public async Task<Domain.Entities.Classrooms.Classroom> UpdateAsync(AuthenticatedUserObject requesterUser, int itemId, Domain.Entities.Classrooms.Classroom updatedClassroom)
    {
        GenericValidation.CheckOnlyManagerUser(requesterUser.Type);

        // TODO: Validade teacher id with service api

        var classroomCheck = _classroomRepository.GetOneById(itemId);
        if (classroomCheck == null || classroomCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Classroom not found");

        var subjectCheck = _subjectRepository.GetOneById(updatedClassroom.SubjectId);
        if (subjectCheck == null || subjectCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Subject not found");

        updatedClassroom.Id = itemId;
        updatedClassroom.AccountId = classroomCheck.AccountId;
        updatedClassroom.CreationDate = classroomCheck.CreationDate;
        updatedClassroom.CreatorId = classroomCheck.CreatorId;
        updatedClassroom.UpdateDate = DateTime.Now;
        updatedClassroom.UpdaterId = requesterUser.UserId;

        return await _classroomRepository.UpdateAsync(updatedClassroom);
    }

    private async Task UpdateStudentsArrayAsync(int accountId, int classroomId, IList<ClassroomStudent> students)
    {
        _classroomStudentRepository.DeleteAllByClassroomId(classroomId);

        foreach (var student in students)
        {
            var studentCheck = _studentRepository.GetOneById(student.StudentId);
            if (studentCheck != null && studentCheck.AccountId != accountId)
            {
                student.ClassroomId = classroomId;
                await _classroomStudentRepository.InsertAsync(student);
            }
        }
    }
}
