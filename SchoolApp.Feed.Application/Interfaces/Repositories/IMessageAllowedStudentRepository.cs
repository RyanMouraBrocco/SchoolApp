using SchoolApp.Feed.Application.Domain.Dtos;

namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IMessageAllowedStudentRepository
{
    Task<IList<MessageAllowedStudentDto>> GetAllByMessageIdAsync(string messageId);
}
