using Microsoft.Extensions.Options;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Queue.Service;
using SchoolApp.Feed.Queue.Settings;

namespace SchoolApp.Feed.Queue.Repositories;

public class MessageAllowedStudentRepository : RabbitMQService<MessageAllowedStudentDto>
{
    public MessageAllowedStudentRepository(IOptions<RabbitMQSettings> options) : base(options, "message.allowed.student")
    {
    }
}