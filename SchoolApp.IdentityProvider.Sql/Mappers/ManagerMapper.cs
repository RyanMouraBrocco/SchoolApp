using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class ManagerMapper
{
    public static Manager MapToDomain(ManagerDto dto)
    {
        if (dto == null)
            return null;

        return new Manager()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Name = dto.Name,
            DocumentId = dto.DocumentId,
            CreatorId = dto.CreatorId,
            CreationDate = dto.CreationDate,
            Email = dto.Email,
            Password = dto.Password,
            UpdateDate = dto.UpdateDate,
            UpdaterId = dto.UpdaterId,
            FunctionId = dto.FunctionId,
            HiringDate = dto.HiringDate,
            Salary = dto.Salary
        };
    }

    public static ManagerDto MapToDto(Manager domain)
    {
        if (domain == null)
            return null;

        return new ManagerDto()
        {
            Id = domain.Id,
            Name = domain.Name,
            AccountId = domain.AccountId,
            DocumentId = domain.DocumentId,
            CreatorId = domain.CreatorId,
            CreationDate = domain.CreationDate,
            Email = domain.Email,
            Password = domain.Password,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId,
            FunctionId = domain.FunctionId,
            HiringDate = domain.HiringDate,
            Salary = domain.Salary
        };
    }
}
