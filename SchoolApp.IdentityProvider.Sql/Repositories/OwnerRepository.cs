using Microsoft.EntityFrameworkCore;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;
using SchoolApp.IdentityProvider.Sql.Mappers;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class OwnerRepository : UserRepository<OwnerDto, Owner>, IOwnerRepository
{
    public OwnerRepository(SchoolAppIdentityProviderContext context) : base(context, OwnerMapper.MapToDomain, OwnerMapper.MapToDto)
    {
    }
}
