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
    private readonly IMessageAllowedPermissionRepository _messageAllowedPermissionRepository;
    private readonly IMessageAllowedClassroomRepository _messageAllowedClassroomRepository;
    private readonly IMessageAllowedStudentRepository _messageAllowedStudentRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IStudentRepository _studentRepository;

    public MessageService(IMessageRepository messageRepository,
                          IMessageAllowedPermissionRepository messageAllowedPermissionRepository,
                          IMessageAllowedClassroomRepository messageAllowedClassroomRepository,
                          IMessageAllowedStudentRepository messageAllowedStudentRepository,
                          IClassroomRepository classroomRepository,
                          IStudentRepository studentRepository)
    {
        _messageRepository = messageRepository;
        _messageAllowedPermissionRepository = messageAllowedPermissionRepository;
        _messageAllowedClassroomRepository = messageAllowedClassroomRepository;
        _messageAllowedStudentRepository = messageAllowedStudentRepository;
        _classroomRepository = classroomRepository;
        _studentRepository = studentRepository;
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

        if (insertedMessage.MessageId == null)
            await UpdateAllowedPermissionsAsync(requesterUser, insertedMessage, allowedClassrooms, allowedStudents);

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

    public async Task<IList<Message>> GetAllMainMessagesAsync(AuthenticatedUserObject requesterUser, int top, int skip)
    {
        if (requesterUser.Type == Shared.Utils.Enums.UserTypeEnum.Owner)
        {
            var resultMessages = new List<Message>();
            var internalMessages = (IList<Message>)new List<Message>();
            int internalSkip = skip;
            do
            {
                internalMessages = _messageRepository.GetAllMainMessages(requesterUser.AccountId, top, internalSkip);
                var userClassrooms = await _classroomRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
                var userStudents = await _classroomRepository.GetAllByOwnerIdAsync(requesterUser.UserId);

                foreach (var internalMessage in internalMessages)
                {
                    var allowedClassroomPermission = await _messageAllowedClassroomRepository.GetAllByMessageIdAsync(internalMessage.MessageId);
                    var allowedStudentPermission = await _messageAllowedStudentRepository.GetAllByMessageIdAsync(internalMessage.MessageId);

                    if (allowedClassroomPermission.Count == 0 && allowedStudentPermission.Count == 0)
                        resultMessages.Add(internalMessage);
                    else if (allowedStudentPermission.Any(x => userStudents.Select(x => x.Id).Contains(x.StudentId)))
                        resultMessages.Add(internalMessage);
                    else if (allowedClassroomPermission.Any(x => userClassrooms.Select(x => x.Id).Contains(x.ClassroomId)))
                        resultMessages.Add(internalMessage);
                }

                internalSkip += top;
            } while (internalMessages.Count == top && resultMessages.Count < top);

            return resultMessages;
        }
        else
            return _messageRepository.GetAllMainMessages(requesterUser.AccountId, top, skip);
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

        if (updatedMessage.MessageId == null)
            await UpdateAllowedPermissionsAsync(requesterUser, resultMessage, allowedClassrooms, allowedStudents);

        return resultMessage;
    }

    private async Task UpdateAllowedPermissionsAsync(AuthenticatedUserObject requesterUser, Message message, IList<MessageAllowedClassroomDto> allowedClassrooms, IList<MessageAllowedStudentDto> allowedStudents)
    {
        var validAllowedClassrooms = new List<MessageAllowedClassroomDto>();
        var validAllowedStudents = new List<MessageAllowedStudentDto>();

        if (requesterUser.Type == Shared.Utils.Enums.UserTypeEnum.Owner)
        {
            foreach (var allowedClassroom in allowedClassrooms)
            {
                var classroomCheck = await _classroomRepository.GetOneByIdAsync(allowedClassroom.ClassroomId);
                if (classroomCheck != null && classroomCheck.AccountId == requesterUser.AccountId)
                {
                    allowedClassroom.MessageId = message.Id;
                    validAllowedClassrooms.Add(allowedClassroom);
                }
            }

            foreach (var allowedStudent in allowedStudents)
            {
                var studentCheck = await _studentRepository.GetOneByIdAsync(allowedStudent.StudentId);
                if (studentCheck != null && studentCheck.AccountId == requesterUser.AccountId)
                {
                    allowedStudent.MessageId = message.Id;
                    validAllowedStudents.Add(allowedStudent);
                }
            }
        }
        else if (requesterUser.Type == Shared.Utils.Enums.UserTypeEnum.Teacher)
        {
            var allStudents = await _studentRepository.GetAllByTeacherIdAsync(requesterUser.UserId);
            var allClassrooms = await _classroomRepository.GetAllByTeacherIdAsync(requesterUser.UserId);

            foreach (var allowedClassroom in allowedClassrooms.Join(allClassrooms, x => x.ClassroomId, y => y.Id, (x, y) => x))
            {
                allowedClassroom.MessageId = message.Id;
                validAllowedClassrooms.Add(allowedClassroom);
            }

            foreach (var allowedStudent in allowedStudents.Join(allClassrooms, x => x.StudentId, y => y.Id, (x, y) => x))
            {
                allowedStudent.MessageId = message.Id;
                validAllowedStudents.Add(allowedStudent);
            }
        }
        else
            throw new UnauthorizedAccessException("This user type can't set permission in message");

        _messageAllowedPermissionRepository.Send(message.Id, validAllowedClassrooms, validAllowedStudents);
    }
}
