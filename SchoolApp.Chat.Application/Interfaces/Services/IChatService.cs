using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Chat.Application.Interfaces.Services;

public interface IChatService
{
    IList<Domain.Entities.Chat> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<Domain.Entities.Chat> CreateAsync(AuthenticatedUserObject requesterUser, Domain.Entities.Chat newChat);
}
