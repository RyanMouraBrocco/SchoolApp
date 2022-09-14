using SchoolApp.Classroom.Application.Domain.Entities.OwnerTypes;
using SchoolApp.Classroom.Sql.Dtos.OwnerTypes;

namespace SchoolApp.Classroom.Sql.Mappers.OwnerTypes;

public static class OwnerTypeMapper
{
    public static OwnerType MapToDomain(OwnerTypeDto dto)
    {
        if (dto == null)
            return null;

        return new OwnerType()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            Name = dto.Name,
            UpdaterId = dto.UpdaterId,
            UpdateDate = dto.UpdateDate
        };
    }

    public static OwnerTypeDto MapToDto(OwnerType domain)
    {
        if (domain == null)
            return null;

        return new OwnerTypeDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            Name = domain.Name,
            UpdaterId = domain.UpdaterId,
            UpdateDate = domain.UpdateDate
        };
    }
}
