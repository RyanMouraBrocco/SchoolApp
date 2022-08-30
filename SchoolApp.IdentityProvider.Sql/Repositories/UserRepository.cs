using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class UserRepository : BaseIdentityRepository<UserDto, User>, IUserRepository
{
    public UserRepository(SchoolAppContext context) : base(context, UserMapper.MapToDomain, UserMapper.MapToDto)
    {
    }

    public User GetOneByEmail(string email)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Email.Equals(email)));
    }
}
