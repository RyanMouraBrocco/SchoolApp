using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.Classroom.Application.Services;

public class ClassroomStudentGradeService : GradeService<ClassroomStudentGrade>
{
    private readonly IClassroomService _classroomService;
    private readonly IClassroomStudentRepository _classroomStudentRepository;
    public ClassroomStudentGradeService(IClassroomStudentGradeRepository classroomStudentGradeRepository,
                                        IStudentService studentService,
                                        IClassroomService classroomService,
                                        IClassroomStudentRepository classroomStudentRepository) : base(classroomStudentGradeRepository, studentService)
    {
        _classroomService = classroomService;
        _classroomStudentRepository = classroomStudentRepository;
    }

    public override Task<ClassroomStudentGrade> CreateAsync(AuthenticatedUserObject requesterUser, ClassroomStudentGrade newGrade)
    {
        var classroomStudent = _classroomStudentRepository.GetOneById(newGrade.ClassroomStudentId);
        if (classroomStudent == null)
            throw new UnauthorizedAccessException("ClassroomStudent relation not found");

        var classroomCheck = _classroomService.GetOneById(requesterUser, classroomStudent.ClassroomId);
        if (classroomCheck == null)
            throw new UnauthorizedAccessException("Classroom not found");

        return base.CreateAsync(requesterUser, newGrade);
    }

    public override Task<ClassroomStudentGrade> UpdateAsync(AuthenticatedUserObject requesterUser, int gradeId, ClassroomStudentGrade updatedGrade)
    {
        var gradeCheck = _gradeRepository.GetOneById(gradeId);
        if (gradeCheck == null || gradeCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Grade not found");

        var classroomStudent = _classroomStudentRepository.GetOneById(gradeCheck.ClassroomStudentId);
        if (classroomStudent == null)
            throw new UnauthorizedAccessException("ClassroomStudent relation not found");

        var classroomCheck = _classroomService.GetOneById(requesterUser, classroomStudent.ClassroomId);
        if (classroomCheck == null)
            throw new UnauthorizedAccessException("Classroom not found");

        updatedGrade.ClassroomStudentId = ((ClassroomStudentGrade)gradeCheck).ClassroomStudentId;

        return base.UpdateAsync(requesterUser, gradeId, updatedGrade);
    }
}