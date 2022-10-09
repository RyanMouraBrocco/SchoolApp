using SchoolApp.Chat.Api.Models;

namespace SchoolApp.Chat.Api.Mappers;

public static class ChatMapper
{
    public static Application.Domain.Entities.Chat MapToChat(this ChatCreateModel model)
    {
        return new Application.Domain.Entities.Chat()
        {
            User2Id = model.UserId,
            User2Type = model.UserType
        };
    }
}
