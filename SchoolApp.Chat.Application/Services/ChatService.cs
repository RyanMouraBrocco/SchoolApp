using SchoolApp.Chat.Application.Interfaces.Repositories;
using SchoolApp.Chat.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Chat.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public IList<Domain.Entities.Chat> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return _chatRepository.GetAllByUserId(requesterUser.UserId, requesterUser.Type, top, skip);
    }

    public async Task<Domain.Entities.Chat> CreateAsync(AuthenticatedUserObject requesterUser, Domain.Entities.Chat newChat)
    {
        var userCheck = await _userRepository.GetOneByIdAsync(newChat.User2Id, newChat.User2Type);
        if (userCheck == null || userCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("User not found");

        var chatCheck = _chatRepository.GetOneByUsers(requesterUser.UserId, requesterUser.Type, newChat.User2Id, newChat.User2Type);
        if (chatCheck != null)
            return chatCheck;

        newChat.User1Id = requesterUser.UserId;
        newChat.User2Type = requesterUser.Type;
        newChat.AccountId = requesterUser.AccountId;
        newChat.CreationDate = DateTime.Now;

        return await _chatRepository.InsertAsync(newChat);
    }
}
