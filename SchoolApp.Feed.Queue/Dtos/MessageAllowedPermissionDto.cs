using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Queue.Dtos;

public class MessageAllowedPermissionDto
{
    public IList<MessageAllowedClassroomDto> AllowedClassrooms { get; set; }
    public IList<MessageAllowedStudentDto> AllowedStudents { get; set; }
}
