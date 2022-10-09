using SchoolApp.Chat.NoSql.Dtos;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Chat.NoSql.Mappers;

public static class ChatMapper
{
    public static Application.Domain.Entities.Chat MapToDomain(ChatDto dto)
    {
        return new Application.Domain.Entities.Chat()
        {
            AccountId = dto.AccountId,
            CreationDate = dto.CreationDate,
            Id = dto.Id,
            User1Id = dto.User1Id,
            User1Type = (UserTypeEnum)dto.User1Type,
            User2Id = dto.User2Id,
            User2Type = (UserTypeEnum)dto.User2Type
        };
    }

    public static ChatDto MapToDto(Application.Domain.Entities.Chat domain)
    {
        return new ChatDto()
        {
            AccountId = domain.AccountId,
            CreationDate = domain.CreationDate,
            Id = domain.Id,
            User1Id = domain.User1Id,
            User1Type = (int)domain.User1Type,
            User2Id = domain.User2Id,
            User2Type = (int)domain.User2Type
        };
    }
}
