using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Feed.Application.Interfaces.Repositories;

public interface IMessageRepository : ICrudRepository<Message, string>
{
    IList<Message> GetAllMainMessages(int accountId, int top, int skip);
}
