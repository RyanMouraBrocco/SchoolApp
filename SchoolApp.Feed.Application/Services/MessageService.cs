using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Feed.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Message> CreateAsync(AuthenticatedUserObject requesterUser, Message newMessage)
    {
        if (string.IsNullOrEmpty(newMessage.MessageId))
            GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);
        else
        {
            var originMessageCheck = _messageRepository.GetOneById(newMessage.Id);
            if (originMessageCheck == null || originMessageCheck.AccountId != requesterUser.AccountId)
                throw new UnauthorizedAccessException("Message not found");
        }

        if (string.IsNullOrEmpty(newMessage.Text?.Trim()))
            throw new FormatException("Text can't be null or empty");

        newMessage.AccountId = requesterUser.AccountId;
        newMessage.CreatorId = requesterUser.UserId;
        newMessage.CreationDate = DateTime.Now;
        newMessage.UpdaterId = null;
        newMessage.UpdateDate = null;

        return await _messageRepository.InsertAsync(newMessage);
    }

    public async Task DeleteAsync(AuthenticatedUserObject requesterUser, string messageId)
    {
        var messageCheck = _messageRepository.GetOneById(messageId);
        if (messageCheck == null || messageCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Message not found");

        if (messageCheck.MessageId != null)
            GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        messageCheck.UpdaterId = requesterUser.UserId;
        messageCheck.UpdateDate = DateTime.Now;

        await _messageRepository.UpdateAsync(messageCheck);
        await _messageRepository.DeleteAsync(messageId);
    }

    public IList<Message> GetAll(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        return _messageRepository.GetAll(requesterUser.AccountId, top, skip);
    }

    public async Task<Message> UpdateAsync(AuthenticatedUserObject requesterUser, string messageId, Message updatedMessage)
    {
        var messageCheck = _messageRepository.GetOneById(messageId);
        if (messageCheck == null || messageCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("Message not found");

        if (string.IsNullOrEmpty(messageCheck.MessageId))
            GenericValidation.CheckOnlyTeacherAndManagerUser(requesterUser.Type);

        if (string.IsNullOrEmpty(updatedMessage.Text?.Trim()))
            throw new FormatException("Text can't be null or empty");

        updatedMessage.Id = messageId;
        updatedMessage.AccountId = messageCheck.AccountId;
        updatedMessage.CreationDate = messageCheck.CreationDate;
        updatedMessage.CreatorId = messageCheck.CreatorId;
        updatedMessage.MessageId = messageCheck.MessageId;
        updatedMessage.UpdateDate = DateTime.Now;
        updatedMessage.UpdaterId = requesterUser.UserId;

        return await _messageRepository.UpdateAsync(updatedMessage);
    }
}
