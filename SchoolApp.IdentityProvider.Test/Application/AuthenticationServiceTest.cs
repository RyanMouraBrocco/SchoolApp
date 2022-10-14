using System.Reflection.Metadata;
using Microsoft.Extensions.Options;
using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.IdentityProvider.Application.Settings;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.IdentityProvider.Test.Application;

public class AuthenticationServiceTest
{
    private readonly Mock<ITeacherRepository> _mockTeacherRepository;
    private readonly Mock<IOwnerRepository> _mockOwnerRepository;
    private readonly Mock<IManagerRepository> _mockManagerRepository;
    private readonly Mock<IOptions<AuthenticationSettings>> _mockAuthenticationSettings;

    public AuthenticationServiceTest()
    {
        _mockTeacherRepository = new Mock<ITeacherRepository>();
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        _mockManagerRepository = new Mock<IManagerRepository>();
        _mockAuthenticationSettings = new Mock<IOptions<AuthenticationSettings>>();
        _mockAuthenticationSettings.Setup(x => x.Value).Returns(new AuthenticationSettings() { Key = "IdP Test DHASUDHSADNSAIODHSUIADGSAUNSJFBSDJFDUFGWIFEWJ", ExpirationTimeInMinutes = 1, Issuer = "Idp Test" });
    }

    [Fact]
    public void Login_ManagerSuccessfullyAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Manager()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(password),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act
        var result = authenticationService.Login(email, password, UserTypeEnum.Manager);

        // Assert
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);

        Assert.NotNull(result.Key);
        Assert.Null(result.User.Password);
        Assert.Equal(userSavedInDatabase.Id, result.User.Id);
        Assert.Equal(userSavedInDatabase.AccountId, result.User.AccountId);
        Assert.Equal(userSavedInDatabase.CreationDate, result.User.CreationDate);
        Assert.Equal(userSavedInDatabase.CreatorId, result.User.CreatorId);
        Assert.Equal(userSavedInDatabase.DocumentId, result.User.DocumentId);
        Assert.Equal(userSavedInDatabase.Email, result.User.Email);
        Assert.Equal(userSavedInDatabase.Name, result.User.Name);
        Assert.Equal(userSavedInDatabase.UpdateDate, result.User.UpdateDate);
        Assert.Equal(userSavedInDatabase.UpdaterId, result.User.UpdaterId);
    }

    [Fact]
    public void Login_OwnerSuccessfullyAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Owner()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(password),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act
        var result = authenticationService.Login(email, password, UserTypeEnum.Owner);

        // Assert
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);

        Assert.NotNull(result.Key);
        Assert.Null(result.User.Password);
        Assert.Equal(userSavedInDatabase.Id, result.User.Id);
        Assert.Equal(userSavedInDatabase.AccountId, result.User.AccountId);
        Assert.Equal(userSavedInDatabase.CreationDate, result.User.CreationDate);
        Assert.Equal(userSavedInDatabase.CreatorId, result.User.CreatorId);
        Assert.Equal(userSavedInDatabase.DocumentId, result.User.DocumentId);
        Assert.Equal(userSavedInDatabase.Email, result.User.Email);
        Assert.Equal(userSavedInDatabase.Name, result.User.Name);
        Assert.Equal(userSavedInDatabase.UpdateDate, result.User.UpdateDate);
        Assert.Equal(userSavedInDatabase.UpdaterId, result.User.UpdaterId);
    }

    [Fact]
    public void Login_TeacherSuccessfullyAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Teacher()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(password),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act
        var result = authenticationService.Login(email, password, UserTypeEnum.Teacher);

        // Assert
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);

        Assert.NotNull(result.Key);
        Assert.Null(result.User.Password);
        Assert.Equal(userSavedInDatabase.Id, result.User.Id);
        Assert.Equal(userSavedInDatabase.AccountId, result.User.AccountId);
        Assert.Equal(userSavedInDatabase.CreationDate, result.User.CreationDate);
        Assert.Equal(userSavedInDatabase.CreatorId, result.User.CreatorId);
        Assert.Equal(userSavedInDatabase.DocumentId, result.User.DocumentId);
        Assert.Equal(userSavedInDatabase.Email, result.User.Email);
        Assert.Equal(userSavedInDatabase.Name, result.User.Name);
        Assert.Equal(userSavedInDatabase.UpdateDate, result.User.UpdateDate);
        Assert.Equal(userSavedInDatabase.UpdaterId, result.User.UpdaterId);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public void Login_EmailNotFoundAsync(UserTypeEnum userType)
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);
        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Teacher>(null);
        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Owner>(null);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => authenticationService.Login(email, password, userType));
        switch (userType)
        {
            case UserTypeEnum.Manager:
                _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
                _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                break;
            case UserTypeEnum.Owner:
                _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
                _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                break;
            case UserTypeEnum.Teacher:
                _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
                _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
                break;
            default:
                throw new NotImplementedException("User type not exists");
        }

    }

    [Fact]
    public void Login_ManagerWrongPasswordAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Manager()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1234"),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => authenticationService.Login(email, password, UserTypeEnum.Manager));
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Login_OwnerWrongPasswordAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Owner()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1234"),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => authenticationService.Login(email, password, UserTypeEnum.Owner));
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Login_TeacherWrongPasswordAsync()
    {
        // Arrange
        var email = "e@e.com";
        var password = "12345678";

        var userSavedInDatabase = new Teacher()
        {
            Id = 1,
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            DocumentId = "1234",
            Email = "e@e.com",
            Name = "User Name",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1234"),
            UpdateDate = null,
            UpdaterId = null
        };

        _mockTeacherRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(userSavedInDatabase);

        var authenticationService = new AuthenticationService(_mockTeacherRepository.Object, _mockOwnerRepository.Object, _mockManagerRepository.Object, _mockAuthenticationSettings.Object);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => authenticationService.Login(email, password, UserTypeEnum.Teacher));
        _mockManagerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Never);
        _mockTeacherRepository.Verify(x => x.GetOneByEmail(It.IsAny<string>()), Times.Once);
    }
}
