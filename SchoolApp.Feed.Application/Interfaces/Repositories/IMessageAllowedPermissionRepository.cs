using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IMessageAllowedPermissionRepository
{
    void Send(string messageId, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents);
}
