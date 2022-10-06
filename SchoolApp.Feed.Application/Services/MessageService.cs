using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Feed.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageAllowedClassroomRepository _messageAllowedClassroomRepository;
    private readonly IMessageAllowedStudentRepository _messageAllowedStudentRepository;

    public MessageService(IMessageRepository messageRepository,
                          IMessageAllowedClassroomRepository messageAllowedClassroomRepository,
                          IMessageAllowedStudentRepository messageAllowedStudentRepository)
    {
        _messageRepository = messageRepository;
        _messageAllowedClassroomRepository = messageAllowedClassroomRepository;
        _messageAllowedStudentRepository = messageAllowedStudentRepository;
    }

    public async Task<Message> CreateAsync(AuthenticatedUserObject requesterUser, Message newMessage, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
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

        var insertedMessage = await _messageRepository.InsertAsync(newMessage);

        UpdateAllowedPermissions(insertedMessage, allowedClassrooms, allowedStudents);

        return insertedMessage;
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

    public async Task<Message> UpdateAsync(AuthenticatedUserObject requesterUser, string messageId, Message updatedMessage, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
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

        var resultMessage = await _messageRepository.UpdateAsync(updatedMessage);

        UpdateAllowedPermissions(resultMessage, allowedClassrooms, allowedStudents);

        return resultMessage;
    }

    private void UpdateAllowedPermissions(Message message, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
    {
        foreach (var allowedClassroom in allowedClassrooms)
        {
            allowedClassroom.MessageId = message.Id;
            allowedClassroom.Message = message;
            _messageAllowedClassroomRepository.Send(allowedClassroom);
        }

        foreach (var allowedStudent in allowedStudents)
        {
            allowedStudent.MessageId = message.Id;
            allowedStudent.Message = message;
            _messageAllowedStudentRepository.Send(allowedStudent);
        }
    }
}
