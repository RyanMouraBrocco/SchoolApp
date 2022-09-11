using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Sql.Dtos.Grades;

namespace SchoolApp.Classroom.Sql.Mappers.Grades;

public static class ActivityAnswerGradeMapper
{
    public static ActivityAnswerGrade MapToDomain(ActivityAnswerGradeDto dto)
    {
        return new ActivityAnswerGrade()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            ActivityAnswerId = dto.ActivityAnswerId,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            StudentId = dto.StudentId,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate
        };
    }

    public static ActivityAnswerGradeDto MapToDto(ActivityAnswerGrade domain)
    {
        return new ActivityAnswerGradeDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            ActivityAnswerId = domain.ActivityAnswerId,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            StudentId = domain.StudentId,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate
        };
    }
}
