using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.NoSql.Dtos.Answers;

namespace SchoolApp.Activity.NoSql.Mappers.Activity;

public static class ActivityAnswerVersionMapper
{
    public static ActivityAnswerVersion MapToDomain(ActivityAnswerVersionDto dto)
    {
        if (dto == null)
            return null;

        return new ActivityAnswerVersion()
        {
            Id = dto.Id.ToString(),
            ActivityAnswerId = dto.ActivityAnswerId.ToString(),
            CreationDate = dto.CreationDate,
            Text = dto.Text
        };
    }

    public static ActivityAnswerVersionDto MapToDto(ActivityAnswerVersion domain)
    {
        if (domain == null)
            return null;

        return new ActivityAnswerVersionDto()
        {
            Id = domain.Id != null ? new MongoDB.Bson.ObjectId(domain.Id) : MongoDB.Bson.ObjectId.GenerateNewId(),
            ActivityAnswerId = new MongoDB.Bson.ObjectId(domain.ActivityAnswerId),
            CreationDate = domain.CreationDate,
            Text = domain.Text
        };
    }
}
