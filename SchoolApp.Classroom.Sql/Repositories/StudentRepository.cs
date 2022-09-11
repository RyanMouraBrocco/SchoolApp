using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Mappers.Students;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class StudentRepository : BaseMainEntityRepository<StudentDto, Student, SchoolAppClassroomContext>, IStudentRepository
{
    public StudentRepository(SchoolAppClassroomContext context) : base(context, StudentMapper.MapToDomain, StudentMapper.MapToDto)
    {
    }
}
