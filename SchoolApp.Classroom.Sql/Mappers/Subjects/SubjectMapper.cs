using System.Runtime.CompilerServices;
using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Sql.Dtos.Subjects;

namespace SchoolApp.Classroom.Sql.Mappers.Subjects;

public static class SubjectMapper
{
    public static Subject MapToDomain(SubjectDto dto)
    {
        return new Subject()
        {
            Id = dto.Id,
            AccountId = dto.AccountId,
            Name = dto.Name,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            UpdateDate = dto.UpdateDate,
            UpdaterId = dto.UpdaterId
        };
    }

    public static SubjectDto MapToDto(Subject domain)
    {
        return new SubjectDto()
        {
            Id = domain.Id,
            AccountId = domain.AccountId,
            Name = domain.Name,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId
        };
    }
}
