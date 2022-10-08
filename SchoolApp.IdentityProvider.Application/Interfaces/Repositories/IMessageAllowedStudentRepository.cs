using SchoolApp.IdentityProvider.Application.Domain.Dtos;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IMessageAllowedStudentRepository : ICrudRepository<MessageAllowedStudentDto, int>
{
    void DeleteAllByMessageId(string messageId);
}
