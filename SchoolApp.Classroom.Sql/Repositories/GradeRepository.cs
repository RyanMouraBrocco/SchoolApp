using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public abstract class GradeRepository<TGradeDto, TGrade> : BaseMainEntityRepository<TGradeDto, TGrade, SchoolAppClassroomContext>, IGradeRepository<TGrade> where TGrade : Grade where TGradeDto : GradeDto
{
    public GradeRepository(SchoolAppClassroomContext context, Func<TGradeDto, TGrade> mapToDomain, Func<TGrade, TGradeDto> mapToDto) : base(context, mapToDomain, mapToDto)
    {
    }
}
