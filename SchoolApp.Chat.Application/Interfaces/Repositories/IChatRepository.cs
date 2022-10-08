using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Chat.Application.Interfaces.Repositories;

public interface IChatRepository : ICrudRepository<Application.Domain.Entities.Chat, string>
{
    Domain.Entities.Chat GetOneByUsers(int userId, UserTypeEnum type, int user2Id, UserTypeEnum user2Type);
    IList<Domain.Entities.Chat> GetAllByUserId(int userId, UserTypeEnum type, int top, int skip);
}
