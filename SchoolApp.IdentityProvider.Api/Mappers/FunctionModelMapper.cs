using SchoolApp.IdentityProvider.Api.Models.Functions;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;

namespace SchoolApp.IdentityProvider.Api.Mappers;

public static class FunctionModelMapper
{
    public static Function MapToFunction(this FunctionModel model)
    {
        return new Function()
        {
            Name = model.Name,
            Description = model.Description
        };
    }
}
