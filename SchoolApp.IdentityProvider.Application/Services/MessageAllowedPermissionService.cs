using SchoolApp.IdentityProvider.Application.Domain.Dtos;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.Application.Services;

public class MessageAllowedPermissionService : IMessageAllowedPermissionService
{
    private readonly IMessageAllowedClassroomRepository _messageAllowedClassroomRepository;
    private readonly IMessageAllowedStudentRepository _messageAllowedStudentRepository;

    public MessageAllowedPermissionService(IMessageAllowedClassroomRepository messageAllowedClassroomRepository,
                                           IMessageAllowedStudentRepository messageAllowedStudentRepository)
    {
        _messageAllowedClassroomRepository = messageAllowedClassroomRepository;
        _messageAllowedStudentRepository = messageAllowedStudentRepository;
    }

    public async Task SyncPermissionAsync(string messageId, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
    {
        _messageAllowedClassroomRepository.DeleteAllByMessageId(messageId);
        _messageAllowedStudentRepository.DeleteAllByMessageId(messageId);

        foreach (var allowedClassroom in allowedClassrooms)
        {
            await _messageAllowedClassroomRepository.InsertAsync(allowedClassroom);
        }

        foreach (var allowedStudent in allowedStudents)
        {
            await _messageAllowedStudentRepository.InsertAsync(allowedStudent);
        }
    }
}
