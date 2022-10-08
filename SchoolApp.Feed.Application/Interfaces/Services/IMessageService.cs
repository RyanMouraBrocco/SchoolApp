using SchoolApp.Shared.Utils.Interfaces;
using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Shared.Authentication;
using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Application.Interfaces.Services;

public interface IMessageService
{
    Task<Message> CreateAsync(AuthenticatedUserObject requesterUser, Message newEntity, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents);
    Task<Message> UpdateAsync(AuthenticatedUserObject requesterUser, string itemId, Message updatedEntity, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents);
    Task<IList<Message>> GetAllMainMessagesAsync(AuthenticatedUserObject requesterUser, int top, int skip);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, string itemId);
}
