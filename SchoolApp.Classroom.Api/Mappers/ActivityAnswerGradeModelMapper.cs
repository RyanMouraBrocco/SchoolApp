using SchoolApp.Classroom.Api.Models.Grades;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;

namespace SchoolApp.Classroom.Api.Mappers;

public static class ActivityAnswerGradeModelMapper
{
    public static ActivityAnswerGrade MapToActivityAnswerGrade(this ActivityAnswerGradeCreateModel model)
    {
        return new ActivityAnswerGrade()
        {
            StudentId = model.StudentId,
            Value = model.Value,
            ActivityAnswerId = model.ActivityAnswerId
        };
    }

    public static ActivityAnswerGrade MapToActivityAnswerGrade(this ActivityAnswerGradeUpdateModel model)
    {
        return new ActivityAnswerGrade()
        {
            Value = model.Value
        };
    }
}
