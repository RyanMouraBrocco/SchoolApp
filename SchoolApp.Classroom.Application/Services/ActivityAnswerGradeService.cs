using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.Classroom.Application.Services;

public class ActivityAnswerGradeService : GradeService<ActivityAnswerGrade>, IActivityAnswerGradeService
{
    public ActivityAnswerGradeService(IActivityAnswerGradeRepository activityAnswerGradeRepository, IStudentService studentService) : base(activityAnswerGradeRepository, studentService)
    {

    }
}
