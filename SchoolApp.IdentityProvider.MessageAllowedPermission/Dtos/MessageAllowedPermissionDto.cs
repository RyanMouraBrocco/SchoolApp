using SchoolApp.IdentityProvider.Application.Domain.Dtos;

namespace SchoolApp.IdentityProvider.MessageAllowedPermission.Dtos;

public class MessageAllowedPermissionDto
{
    public string MessageId { get; set; }
    public IList<MessageAllowedClassroomDto> AllowedClassrooms { get; set; }
    public IList<MessageAllowedStudentDto> AllowedStudents { get; set; }
}
