using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Classrooms;
using SchoolApp.Classroom.Sql.Mappers.Classrooms;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class ClassroomRepository : BaseMainEntityRepository<ClassroomDto, Application.Domain.Entities.Classrooms.Classroom, SchoolAppClassroomContext>, IClassroomRepository
{
    public ClassroomRepository(SchoolAppClassroomContext context) : base(context, ClassroomMapper.MapToDomain, ClassroomMapper.MapToDto)
    {
    }
}
