using Microsoft.Extensions.Options;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Queue.Dtos;
using SchoolApp.Feed.Queue.Service;
using SchoolApp.Feed.Queue.Settings;

namespace SchoolApp.Feed.Queue.Repositories;

public class MessageAllowedPermissionRepository : RabbitMQService<MessageAllowedPermissionDto>, IMessageAllowedPermissionRepository
{
    public MessageAllowedPermissionRepository(IOptions<RabbitMQSettings> options) : base(options, "message.allowed.permission")
    {
    }

    public void Send(IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
    {
        var genericAllowedPermission = new MessageAllowedPermissionDto()
        {
            AllowedClassrooms = allowedClassrooms,
            AllowedStudents = allowedStudents
        };

        Send(genericAllowedPermission);
    }
}
