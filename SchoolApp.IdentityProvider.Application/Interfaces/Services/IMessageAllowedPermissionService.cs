using SchoolApp.IdentityProvider.Application.Domain.Dtos;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IMessageAllowedPermissionService
{
    Task SyncPermissionAsync(string messageId, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents);
}
