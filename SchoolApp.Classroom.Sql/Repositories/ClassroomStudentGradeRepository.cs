using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Grades;
using SchoolApp.Classroom.Sql.Mappers.Grades;

namespace SchoolApp.Classroom.Sql.Repositories;

public class ClassroomStudentGradeRepository : GradeRepository<ClassroomStudentGradeDto, ClassroomStudentGrade>, IClassroomStudentGradeRepository
{
    public ClassroomStudentGradeRepository(SchoolAppClassroomContext context) : base(context, ClassroomStudentGradeMapper.MapToDomain, ClassroomStudentGradeMapper.MapToDto)
    {
    }
}
