using SchoolApp.Feed.Api.Models;
using SchoolApp.Feed.Application.Domain.Entities;

namespace SchoolApp.Feed.Api.Mappers;

public static class MessageModelMapper
{
    public static Message MapToMessage(this MessageCreateModel model)
    {
        return new Message()
        {
            MessageId = model.MessageId,
            Text = model.Text
        };
    }

    public static Message MapToMessage(this MessageUpdateModel model)
    {
        return new Message()
        {
            Text = model.Text
        };
    }
}
