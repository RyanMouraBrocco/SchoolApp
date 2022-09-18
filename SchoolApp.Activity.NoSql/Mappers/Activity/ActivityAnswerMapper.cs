using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.NoSql.Dtos.Answers;

namespace SchoolApp.Activity.NoSql.Mappers.Activity;

public static class ActivityAnswerMapper
{
    public static ActivityAnswer MapToDomain(ActivityAnswerDto dto)
    {
        if (dto == null)
            return null;

        return new ActivityAnswer()
        {
            Id = dto.Id.ToString(),
            ActivityId = dto.ActivityId.ToString(),
            CreationDate = dto.CreationDate,
            StudentId = dto.StudentId,
            LastReview = ActivityAnswerVersionMapper.MapToDomain(dto.LastReview)
        };
    }

    public static ActivityAnswerDto MapToDto(ActivityAnswer domain)
    {
        if (domain == null)
            return null;

        return new ActivityAnswerDto()
        {
            Id = domain.Id != null ? new MongoDB.Bson.ObjectId(domain.Id) : MongoDB.Bson.ObjectId.GenerateNewId(),
            ActivityId = new MongoDB.Bson.ObjectId(domain.ActivityId),
            CreationDate = domain.CreationDate,
            StudentId = domain.StudentId,
            AccountId = domain.AccountId,
            LastReview = ActivityAnswerVersionMapper.MapToDto(domain.LastReview)
        };
    }
}
