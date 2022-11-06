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
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Once);
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

    [Fact]
    public async Task CreateNewMainMessage_TryToCreateWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

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

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.CreateAsync(requesterUser, newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.InsertAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }


    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task CreateNewResponseMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(new Message() { Id = "1234", AccountId = 1 });

        if (userType == UserTypeEnum.Owner)
        {
            _mockClassroomRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>()));
            _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>()));
            _mockMessageAllowedClassroomRepository.Setup(x => x.GetAllByMessageIdAsync(It.IsAny<string>())).Returns(Task.FromResult<IList<MessageAllowedClassroomDto>>(new List<MessageAllowedClassroomDto>()));
            _mockMessageAllowedStudentRepository.Setup(x => x.GetAllByMessageIdAsync(It.IsAny<string>())).Returns(Task.FromResult<IList<MessageAllowedStudentDto>>(new List<MessageAllowedStudentDto>()));
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
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task CreateNewResponseMessage_TryToCreateInMessageOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(new Message() { Id = "1234", AccountId = 2 });

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.CreateAsync(requesterUser, newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.InsertAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task CreateNewResponseMessage_TryToCreateWithNonExistentMessageAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(new Message() { Id = "1234", AccountId = 2 });

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.CreateAsync(requesterUser, newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.InsertAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, null)]
    [InlineData(UserTypeEnum.Teacher, null)]
    [InlineData(UserTypeEnum.Owner, null)]
    [InlineData(UserTypeEnum.Manager, "")]
    [InlineData(UserTypeEnum.Teacher, "")]
    [InlineData(UserTypeEnum.Owner, "")]
    [InlineData(UserTypeEnum.Manager, " ")]
    [InlineData(UserTypeEnum.Teacher, " ")]
    [InlineData(UserTypeEnum.Owner, " ")]
    public async Task CreateNewResponseMessage_TryToCreateWithNullOrEmptyTextAsync(UserTypeEnum userType, string text)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
            Text = text
        };

        var allowedClassrooms = new List<MessageAllowedClassroomDto>()
        {
            new() { ClassroomId = 1 }
        };

        var allowedStudents = new List<MessageAllowedStudentDto>()
        {
            new() { StudentId = 1 }
        };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(new Message() { Id = "1234", AccountId = 1 });

        if (userType == UserTypeEnum.Owner)
        {
            _mockClassroomRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>()));
            _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>()));
            _mockMessageAllowedClassroomRepository.Setup(x => x.GetAllByMessageIdAsync(It.IsAny<string>())).Returns(Task.FromResult<IList<MessageAllowedClassroomDto>>(new List<MessageAllowedClassroomDto>()));
            _mockMessageAllowedStudentRepository.Setup(x => x.GetAllByMessageIdAsync(It.IsAny<string>())).Returns(Task.FromResult<IList<MessageAllowedStudentDto>>(new List<MessageAllowedStudentDto>()));
        }

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => messageService.CreateAsync(requesterUser, newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.InsertAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateNewMainMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = requesterUser.UserId, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = null };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);
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
        var result = await messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents);

        // Assert
        Assert.Equal(newMessage.Text, result.Text);
        Assert.Equal(newMessage.MessageId, result.MessageId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(messageInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(messageInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.NotNull(result.Id);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Once);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Once);
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

    [Fact]
    public async Task UpdateNewMainMessage_TryToCreateWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = requesterUser.UserId, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = null };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task UpdateNewResponseMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = requesterUser.UserId, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act
        var result = await messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents);

        // Assert
        Assert.Equal(newMessage.Text, result.Text);
        Assert.Equal(newMessage.MessageId, result.MessageId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(messageInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(messageInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.NotNull(result.Id);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Once);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, null)]
    [InlineData(UserTypeEnum.Teacher, null)]
    [InlineData(UserTypeEnum.Owner, null)]
    [InlineData(UserTypeEnum.Manager, "")]
    [InlineData(UserTypeEnum.Teacher, "")]
    [InlineData(UserTypeEnum.Owner, "")]
    [InlineData(UserTypeEnum.Manager, " ")]
    [InlineData(UserTypeEnum.Teacher, " ")]
    [InlineData(UserTypeEnum.Owner, " ")]
    public async Task UpdateMessage_TryToUpdateWithInvalidTextAsync(UserTypeEnum userType, string text)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
            Text = text
        };

        var allowedClassrooms = new List<MessageAllowedClassroomDto>()
        {
            new() { ClassroomId = 1 }
        };

        var allowedStudents = new List<MessageAllowedStudentDto>()
        {
            new() { StudentId = 1 }
        };

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = requesterUser.UserId, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task UpdateMessage_TryToUpdateWithNonExistentMessageAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns<Message>(null);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task UpdateMessage_TryToUpdateWithMessageOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        var messageInDatabase = new Message() { Id = "1234", AccountId = 2, CreatorId = requesterUser.UserId, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task UpdateMessage_TryToUpdateWithMessageOfAnotherUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newMessage = new Message()
        {
            MessageId = "1234",
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

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = 2, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.UpdateAsync(requesterUser, "1234", newMessage, allowedClassrooms, allowedStudents));
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageAllowedPermissionRepository.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<List<MessageAllowedClassroomDto>>(), It.IsAny<List<MessageAllowedStudentDto>>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.GetAllByTeacherIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteNewMainMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = 1, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = null };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act
        await messageService.DeleteAsync(requesterUser, "1234");

        // Assert
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Once);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteNewMainMessage_TryToDeleteWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = 1, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = null };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.DeleteAsync(requesterUser, "1234"));
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task DeleteNewResponseMessage_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = 1, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act
        await messageService.DeleteAsync(requesterUser, "1234");

        // Assert
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Once);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task DeleteMessage_TryToDeleteWithNonExistentMessageAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns<Message>(null);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.DeleteAsync(requesterUser, "1234"));
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task DeleteMessage_TryToDeleteWithMessageWithAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var messageInDatabase = new Message() { Id = "1234", AccountId = 2, CreatorId = 1, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.DeleteAsync(requesterUser, "1234"));
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task DeleteMessage_TryToDeleteWithMessageOfAnotherUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var messageInDatabase = new Message() { Id = "1234", AccountId = 1, CreatorId = 2, CreationDate = DateTime.Now.AddDays(-3), Text = "old text", MessageId = "1234" };

        _mockMessageRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(messageInDatabase);

        var messageService = new MessageService(_mockMessageRepository.Object, _mockMessageAllowedPermissionRepository.Object, _mockMessageAllowedClassroomRepository.Object, _mockMessageAllowedStudentRepository.Object, _mockClassroomRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => messageService.DeleteAsync(requesterUser, "1234"));
        _mockMessageRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockMessageRepository.Verify(x => x.UpdateAsync(It.IsAny<Message>()), Times.Never);
        _mockMessageRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }
}
