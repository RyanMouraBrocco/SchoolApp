using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.Classroom.Application.Services;

public class ActivityAnswerGradeService : GradeService<ActivityAnswerGrade>, IActivityAnswerGradeService
{
    private readonly IActivityAnswerRepository _activityAnswerRepository;
    private readonly IClassroomService _classroomService;

    public ActivityAnswerGradeService(IActivityAnswerGradeRepository activityAnswerGradeRepository,
                                      IStudentService studentService,
                                      IActivityAnswerRepository activityAnswerRepository,
                                      IClassroomService classroomService) : base(activityAnswerGradeRepository, studentService)
    {
        _activityAnswerRepository = activityAnswerRepository;
        _classroomService = classroomService;
    }

    public override async Task<ActivityAnswerGrade> CreateAsync(AuthenticatedUserObject requesterUser, ActivityAnswerGrade newGrade)
    {
        var activityAnswerCheck = await _activityAnswerRepository.GetOneByIdIncludingActivityAsync(newGrade.ActivityAnswerId);
        if (activityAnswerCheck == null)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        var classroomCheck = _classroomService.GetOneById(requesterUser, activityAnswerCheck.Activity.ClassroomId);
        if (classroomCheck == null)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        return await base.CreateAsync(requesterUser, newGrade);
    }

    public override async Task<ActivityAnswerGrade> UpdateAsync(AuthenticatedUserObject requesterUser, int gradeId, ActivityAnswerGrade updatedGrade)
    {
        var gradeCheck = _gradeRepository.GetOneById(gradeId);
        if (gradeCheck == null || gradeCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Grade not found");

        var activityAnswerCheck = await _activityAnswerRepository.GetOneByIdIncludingActivityAsync(((ActivityAnswerGrade)gradeCheck).ActivityAnswerId);
        if (activityAnswerCheck == null)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        var classroomCheck = _classroomService.GetOneById(requesterUser, activityAnswerCheck.Activity.ClassroomId);
        if (classroomCheck == null)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        return await base.UpdateAsync(requesterUser, gradeId, updatedGrade);
    }
}
