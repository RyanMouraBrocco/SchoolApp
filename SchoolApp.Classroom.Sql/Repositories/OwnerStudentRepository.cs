using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.Students;
using SchoolApp.Classroom.Sql.Mappers.Students;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class OwnerStudentRepository : BaseCrudRepository<OwnerStudentDto, OwnerStudent, SchoolAppClassroomContext>, IOwnerStudentRepository
{
    public OwnerStudentRepository(SchoolAppClassroomContext context) : base(context, OwnerStudentMapper.MapToDomain, OwnerStudentMapper.MapToDto)
    {
    }
}
