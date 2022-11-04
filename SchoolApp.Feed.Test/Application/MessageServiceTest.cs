using Moq;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Domain.Entities;
using SchoolApp.Feed.Application.Interfaces.Repositories;
using SchoolApp.Feed.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Feed.Test.Application;

public class MessageServiceTest
{
    private readonly Mock<IMessageRepository> _mockMessageRepository;
    private readonly Mock<IMessageAllowedPermissionRepository> _mockMessageAllowedPermissionRepository;
    private readonly Mock<IMessageAllowedClassroomRepository> _mockMessageAllowedClassroomRepository;
    private readonly Mock<IMessageAllowedStudentRepository> _mockMessageAllowedStudentRepository;
    private readonly Mock<IClassroomRepository> _mockClassroomRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;

    public MessageServiceTest()
    {
        _mockMessageRepository = new Mock<IMessageRepository>();
        _mockMessageAllowedPermissionRepository = new Mock<IMessageAllowedPermissionRepository>();
        _mockMessageAllowedClassroomRepository = new Mock<IMessageAllowedClassroomRepository>();
        _mockMessageAllowedStudentRepository = new Mock<IMessageAllowedStudentRepository>();
        _mockClassroomRepository = new Mock<IClassroomRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();

        _mockMessageRepository.Setup(x => x.InsertAsync(It.IsAny<Message>())).Returns((Message x) => { x.Id = "1"; return Task.FromResult(x); });
        _mockMessageRepository.Setup(x => x.UpdateAsync(It.IsAny<Message>())).Returns((Message x) => { return Task.FromResult(x); });
        _mockMessageRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()));
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewMainMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = null,
            Text = "text"
        };

        var allowedClassrooms = new List<MessageAllowedClassroomDto>()
        {
            new() { ClassroomId = 1 }
        };

        var allowedStudents = new List<MessageAllowedStudentDto>()
        {
            new() { StudentId = 1 }
        };

        if (userType == UserTypeEnum.Manager)
        {
            _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1 }));
            _mockStudentRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new StudentDto() { Id = 1, AccountId = 1 }));
        }
        else
        {
            _mockClassroomRepository.Setup(x => x.GetAllByTeacherIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1, AccountId = 1 } }));
            _mockStudentRepository.Setup(x => x.GetAllByTeacherIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1, AccountId = 1 } }));
        }

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act
        var result = await messageService.CreateAsync(requesterUser, newMessage, allowedClassrooms, allowedStudents);

        // Assert
        Assert.Equal(newMessage.Text, result.Text);
        Assert.Equal(newMessage.MessageId, result.MessageId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.NotNull(result.Id);
        _mockMessageRepository.Verify(x => x.InsertAsync(It.IsAny<Message>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()));
        if (userType == UserTypeEnum.Manager)
        {
            _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
            _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        }
        else
        {
            _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Once);
            _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
