using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class FunctionMapper
{
    public static Function MapToDomain(FunctionDto dto)
    {
        return new Function()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Name = dto.Name,
            Description = dto.Description,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            UpdateDate = dto.UpdateDate,
            UpdaterId = dto.UpdaterId
        };
    }

    public static FunctionDto MapToDto(Function domain)
    {
        return new FunctionDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            Name = domain.Name,
            Description = domain.Description,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId
        };
    }
}
