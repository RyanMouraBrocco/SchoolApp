using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class ManagerRepository : UserRepository<ManagerDto, Manager>, IManagerRepository
{
    public ManagerRepository(SchoolAppIdentityProviderContext context) : base(context, ManagerMapper.MapToDomain, ManagerMapper.MapToDto)
    {
    }
}
