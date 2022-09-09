using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.Shared.Utils.Sql.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public abstract class UserRepository<TDto, TDomain> : BaseMainEntityRepository<TDto, TDomain, SchoolAppIdentityProviderContext>, IUserRepository<TDomain> where TDto : UserDto
                                                                                                                                          where TDomain : User
{
    public UserRepository(SchoolAppIdentityProviderContext context,
                          Func<TDto, TDomain> mapToDomain,
                          Func<TDomain, TDto> mapToDto) : base(context, mapToDomain, mapToDto)
    {
    }

    public TDomain GetOneByEmail(string email)
    {
        return MapToDomain(_dbSet.AsNoTracking().FirstOrDefault(x => x.Email.Equals(email) && !x.Deleted));
    }
}
