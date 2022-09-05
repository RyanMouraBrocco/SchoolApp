using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;
using SchoolApp.IdentityProvider.Sql.Mappers;
using SchoolApp.IdentityProvider.Sql.Repositories.Base;

namespace SchoolApp.IdentityProvider.Sql.Repositories;

public class FunctionRepository : BaseMainEntityRepository<FunctionDto, Function>, IFunctionRepository
{
    public FunctionRepository(SchoolAppContext context) : base(context, FunctionMapper.MapToDomain, FunctionMapper.MapToDto)
    {
    }
}
