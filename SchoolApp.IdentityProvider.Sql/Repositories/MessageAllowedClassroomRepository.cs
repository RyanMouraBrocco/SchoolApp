using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Dtos;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class MessageAllowedClassroomRepository : BaseCrudRepository<Dtos.MessageAllowedPermissions.MessageAllowedClassroomDto, Application.Domain.Dtos.MessageAllowedClassroomDto, SchoolAppIdentityProviderContext>, IMessageAllowedClassroomRepository
{
    public MessageAllowedClassroomRepository(SchoolAppIdentityProviderContext context) : base(context, MessageAllowedPermissionMapper.MapToDomain, MessageAllowedPermissionMapper.MapToDto)
    {
    }

    public void DeleteAllByMessageId(string messageId)
    {
        _dbSet.RemoveRange(_context.GetQueryable(_dbSet).Where(x => x.MessageId == messageId));
    }
}
