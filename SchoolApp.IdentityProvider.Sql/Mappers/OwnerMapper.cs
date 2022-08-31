using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Sql.Dtos.Users;

namespace SchoolApp.IdentityProvider.Sql.Mappers;

public static class OwnerMapper
{
    public static Owner MapToDomain(OwnerDto dto)
    {
        if (dto == null)
            return null;

        return new Owner()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Name = dto.Name,
            DocumentId = dto.DocumentId,
            CreatorId = dto.CreatorId,
            CreationDate = dto.CreationDate,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate,
            Email = dto.Email,
            Password = dto.Password,
            IsMainOwner = dto.IsMainOwner
        };
    }

    public static OwnerDto MapToDto(Owner domain)
    {
        if (domain == null)
            return null;
            
        return new OwnerDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            Name = domain.Name,
            DocumentId = domain.DocumentId,
            CreatorId = domain.CreatorId,
            CreationDate = domain.CreationDate,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate,
            Email = domain.Email,
            Password = domain.Password,
            IsMainOwner = domain.IsMainOwner
        };
    }
}
