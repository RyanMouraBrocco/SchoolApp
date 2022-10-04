using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.NoSql.Dtos;

namespace SchoolApp.Feed.NoSql.Mappers;

public static class MessageMapper
{
    public static Message MapToDomain(MessageDto dto)
    {
        if (dto == null)
            return null;

        return new Message()
        {
            AccountId = dto.AccountId,
            CreationDate = dto.CreationDate,
            CreatorId = dto.CreatorId,
            Id = dto.Id.ToString(),
            MessageId = dto.MessageId.ToString(),
            Text = dto.Text,
            UpdateDate = dto.UpdateDate,
            UpdaterId = dto.UpdaterId
        };
    }

    public static MessageDto MapToDto(Message domain)
    {
        if (domain == null)
            return null;

        return new MessageDto()
        {
            AccountId = domain.AccountId,
            CreationDate = domain.CreationDate,
            CreatorId = domain.CreatorId,
            Id = domain.Id != null ? new MongoDB.Bson.ObjectId(domain.Id) : MongoDB.Bson.ObjectId.GenerateNewId(),
            MessageId = domain.MessageId != null ? new MongoDB.Bson.ObjectId(domain.MessageId) : null,
            Text = domain.Text,
            UpdateDate = domain.UpdateDate,
            UpdaterId = domain.UpdaterId
        };
    }
}
