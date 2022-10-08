using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IMessageAllowedClassroomRepository
{
    Task<IList<MessageAllowedClassroomDto>> GetAllByMessageIdAsync(string messageId);
}
