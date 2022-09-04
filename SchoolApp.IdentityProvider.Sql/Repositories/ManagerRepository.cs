using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class ManagerRepository : BaseMainEntityRepository<ManagerDto, Manager>, IManagerRepository
{
    public ManagerRepository(SchoolAppContext context) : base(context, ManagerMapper.MapToDomain, ManagerMapper.MapToDto)
    {
    }

    public Manager GetOneByEmail(string email)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Email.Equals(email) && !x.Deleted));
    }
}
