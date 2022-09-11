using SchoolApp.Classroom.Application.Domain.Entities.OwnerTypes;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Sql.Context;
using SchoolApp.Classroom.Sql.Dtos.OwnerTypes;
using SchoolApp.Classroom.Sql.Mappers.OwnerTypes;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.Classroom.Sql.Repositories;

public class OwnerTypeRepository : BaseMainEntityRepository<OwnerTypeDto, OwnerType, SchoolAppClassroomContext>, IOwnerTypeRepository
{
    public OwnerTypeRepository(SchoolAppClassroomContext context) : base(context, OwnerTypeMapper.MapToDomain, OwnerTypeMapper.MapToDto)
    {
    }
}
