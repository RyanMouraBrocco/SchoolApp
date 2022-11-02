using Moq;
using SchoolApp.Chat.Application.Domain.Dtos;
using SchoolApp.Chat.Application.Interfaces.Repositories;
using SchoolApp.Chat.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Chat.Test.Applications;

public class ChatServiceTest
{
    private readonly Mock<IChatRepository> _mockChatRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;

    public ChatServiceTest()
    {
        _mockChatRepository = new Mock<IChatRepository>();
        _mockUserRepository = new Mock<IUserRepository>();

        _mockChatRepository.Setup(x => x.InsertAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>())).Returns((Chat.Application.Domain.Entities.Chat x) => { x.Id = "1"; return Task.FromResult(x); });
        _mockChatRepository.Setup(x => x.UpdateAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>())).Returns((Chat.Application.Domain.Entities.Chat x) => { return Task.FromResult(x); });
        _mockChatRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()));
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Owner)]
    public async Task CreateChat_SucessfullyAsync(UserTypeEnum userType, UserTypeEnum destinationUserType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockUserRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType)).Returns(Task.FromResult(new UserDto() { Id = 2, AccountId = 1 }));
        _mockChatRepository.Setup(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType)).Returns<Chat.Application.Domain.Entities.Chat>(null);

        var newChat = new Chat.Application.Domain.Entities.Chat()
        {
            User2Id = 2,
            User2Type = destinationUserType
        };

        var chatService = new ChatService(_mockChatRepository.Object, _mockUserRepository.Object);

        // Act
        var result = await chatService.CreateAsync(requesterUser, newChat);

        // Assert
        Assert.Equal(requesterUser.UserId, result.User1Id);
        Assert.Equal(requesterUser.Type, result.User1Type);
        Assert.Equal(newChat.User2Id, result.User2Id);
        Assert.Equal(newChat.User2Type, result.User2Type);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        _mockChatRepository.Verify(x => x.InsertAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>()), Times.Once);
        _mockUserRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType), Times.Once);
        _mockChatRepository.Verify(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Owner)]
    public async Task CreateChat_TryToCreateDuplicateChatAsync(UserTypeEnum userType, UserTypeEnum destinationUserType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var chatInDatabase = new Chat.Application.Domain.Entities.Chat()
        {
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            Id = "12345",
            User1Id = requesterUser.UserId,
            User1Type = requesterUser.Type,
            User2Id = 2,
            User2Type = destinationUserType
        };

        _mockUserRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType)).Returns(Task.FromResult(new UserDto() { Id = 2, AccountId = 1 }));
        _mockChatRepository.Setup(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType)).Returns(chatInDatabase);

        var newChat = new Chat.Application.Domain.Entities.Chat()
        {
            User2Id = 2,
            User2Type = destinationUserType
        };

        var chatService = new ChatService(_mockChatRepository.Object, _mockUserRepository.Object);

        // Act
        var result = await chatService.CreateAsync(requesterUser, newChat);

        // Assert
        Assert.Equal(chatInDatabase.User1Id, result.User1Id);
        Assert.Equal(chatInDatabase.User1Type, result.User1Type);
        Assert.Equal(chatInDatabase.User2Id, result.User2Id);
        Assert.Equal(chatInDatabase.User2Type, result.User2Type);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(chatInDatabase.CreationDate, result.CreationDate);
        _mockChatRepository.Verify(x => x.InsertAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>()), Times.Never);
        _mockUserRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType), Times.Once);
        _mockChatRepository.Verify(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Owner)]
    public async Task CreateChat_TryToCreateWitNonExistentUserAsync(UserTypeEnum userType, UserTypeEnum destinationUserType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockUserRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType)).Returns(Task.FromResult<UserDto>(null));
        _mockChatRepository.Setup(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType)).Returns<Chat.Application.Domain.Entities.Chat>(null);

        var newChat = new Chat.Application.Domain.Entities.Chat()
        {
            User2Id = 2,
            User2Type = destinationUserType
        };

        var chatService = new ChatService(_mockChatRepository.Object, _mockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => chatService.CreateAsync(requesterUser, newChat));
        _mockChatRepository.Verify(x => x.InsertAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>()), Times.Never);
        _mockUserRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType), Times.Once);
        _mockChatRepository.Verify(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Manager, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Teacher, UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner, UserTypeEnum.Owner)]
    public async Task CreateChat_TryToCreateWitUserOfAnotherAccountAsync(UserTypeEnum userType, UserTypeEnum destinationUserType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockUserRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType)).Returns(Task.FromResult(new UserDto() { Id = 2, AccountId = 2 }));
        _mockChatRepository.Setup(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType)).Returns<Chat.Application.Domain.Entities.Chat>(null);

        var newChat = new Chat.Application.Domain.Entities.Chat()
        {
            User2Id = 2,
            User2Type = destinationUserType
        };

        var chatService = new ChatService(_mockChatRepository.Object, _mockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => chatService.CreateAsync(requesterUser, newChat));
        _mockChatRepository.Verify(x => x.InsertAsync(It.IsAny<Chat.Application.Domain.Entities.Chat>()), Times.Never);
        _mockUserRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>(), destinationUserType), Times.Once);
        _mockChatRepository.Verify(x => x.GetOneByUsers(It.IsAny<int>(), userType, It.IsAny<int>(), destinationUserType), Times.Never);
    }
}
