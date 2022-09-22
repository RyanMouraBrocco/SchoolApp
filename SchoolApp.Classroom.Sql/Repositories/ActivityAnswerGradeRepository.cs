using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Classroom.Sql.Mappers.Grades;

namespace SchoolApp.Classroom.Sql.Repositories;

public class ActivityAnswerGradeRepository : GradeRepository<ActivityAnswerGradeDto, ActivityAnswerGrade>, IActivityAnswerGradeRepository
{
    public ActivityAnswerGradeRepository(SchoolAppClassroomContext context) : base(context, ActivityAnswerGradeMapper.MapToDomain, ActivityAnswerGradeMapper.MapToDto)
    {
    }
}
