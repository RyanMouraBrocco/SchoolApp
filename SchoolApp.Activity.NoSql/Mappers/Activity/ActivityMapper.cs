using System.Xml;
using MongoDB.Bson;
using SchoolApp.Activity.NoSql.Dtos.Activities;

namespace SchoolApp.Activity.NoSql.Mappers.Activity;

public static class ActivityMapper
{
    public static Application.Domain.Entities.Activities.Activity MapToDomain(ActivityDto dto)
    {
        if (dto == null)
            return null;

        return new Application.Domain.Entities.Activities.Activity()
        {
            Id = dto.Id.ToString(),
            AccountId = dto.AccountId,
            ClassroomId = dto.ClassroomId,
            Closed = dto.Closed,
            CloseDate = dto.CloseDate,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            Description = dto.Description,
            Name = dto.Name,
            UpdateDate = dto.UpdateDate,
            UpdaterId = dto.UpdaterId
        };
    }

    public static ActivityDto MapToDto(Application.Domain.Entities.Activities.Activity domain)
    {
        if (domain == null)
            return null;

        return new ActivityDto()
        {
            Id = domain.Id != null ? new MongoDB.Bson.ObjectId(domain.Id) : ObjectId.GenerateNewId(),
            AccountId = domain.AccountId,
            ClassroomId = domain.ClassroomId,
            Closed = domain.Closed,
            CloseDate = domain.CloseDate,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            Description = domain.Description,
            Name = domain.Name,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId
        };
    }
}
