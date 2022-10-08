using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class MessageAllowedStudentRepository : BaseCrudRepository<Dtos.MessageAllowedPermissions.MessageAllowedStudentDto, Application.Domain.Dtos.MessageAllowedStudentDto, SchoolAppIdentityProviderContext>, IMessageAllowedStudentRepository
{
    public MessageAllowedStudentRepository(SchoolAppIdentityProviderContext context) : base(context, MessageAllowedPermissionMapper.MapToDomain, MessageAllowedPermissionMapper.MapToDto)
    {
    }
}
