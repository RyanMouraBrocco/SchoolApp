using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.IdentityProvider.Test.Application;

public class OwnerServiceTest
{
    private readonly Mock<IOwnerRepository> _mockOwnerRepository;

    public OwnerServiceTest()
    {
        _mockOwnerRepository = new Mock<IOwnerRepository>();

        _mockOwnerRepository.Setup(x => x.InsertAsync(It.IsAny<Owner>())).Returns((Owner x) => { x.Id = 1; return Task.FromResult(x); });
        _mockOwnerRepository.Setup(x => x.UpdateAsync(It.IsAny<Owner>())).Returns((Owner x) => { return Task.FromResult(x); });
        _mockOwnerRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateNewOwner_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var newOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com"
        };

        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Owner>(null);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act
        var result = await ownerService.CreateAsync(requesterUser, newOwner);

        // Assert
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(newOwner.Email), Times.Once);
        _mockOwnerRepository.Verify(x => x.InsertAsync(newOwner), Times.Once);
        Assert.Equal(newOwner.Name, result.Name);
        Assert.Equal(newOwner.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(newOwner.Email, result.Email);
        Assert.Equal(newOwner.IsMainOwner, result.IsMainOwner);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewOwner_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var newOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com"
        };

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.CreateAsync(requesterUser, newOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(newOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.InsertAsync(newOwner), Times.Never);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("12345678")]
    [InlineData("abcd")]
    [InlineData("abcdefgh")]
    [InlineData("ABCD")]
    [InlineData("ABCDEFGH")]
    [InlineData("aBcD")]
    [InlineData("aBcDeFgH")]
    [InlineData("4bCD")]
    public async Task CreateNewOwner_TryToCreateWithoutASecurityPasswordAsync(string password)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = password,
            Email = "e@e.com"
        };

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => ownerService.CreateAsync(requesterUser, newOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(newOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.InsertAsync(newOwner), Times.Never);
    }

    [Theory]
    [InlineData("Name", null)]
    [InlineData("Name", "")]
    [InlineData("Name", " ")]
    [InlineData("Email", null)]
    [InlineData("Email", "")]
    [InlineData("Email", " ")]
    [InlineData("Password", null)]
    [InlineData("Password", "")]
    [InlineData("Password", " ")]
    [InlineData("DocumentId", null)]
    [InlineData("DocumentId", "")]
    [InlineData("DocumentId", " ")]
    public async Task CreateNewOwner_TryToCreateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com"
        };
        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        typeof(Owner).GetProperty(property).SetValue(newOwner, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => ownerService.CreateAsync(requesterUser, newOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(newOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.InsertAsync(newOwner), Times.Never);
    }

    [Fact]
    public async Task CreateNewOwner_TryToWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com"
        };

        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Owner() { Id = 2 });

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.CreateAsync(requesterUser, newOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(newOwner.Email), Times.Once);
        _mockOwnerRepository.Verify(x => x.InsertAsync(newOwner), Times.Never);
    }

    [Fact]
    public async Task UpdateOwner_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedOwner = new Owner()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com",
        };

        var ownerSaveInDatabase = new Owner()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(purePassword),
            Email = "e@e.com",
            Id = 1
        };

        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Owner>(null);
        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(ownerSaveInDatabase);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act
        var result = await ownerService.UpdateAsync(requesterUser, 1, updatedOwner);

        // Assert
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Once);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Once);
        Assert.Equal(updatedOwner.Name, result.Name);
        Assert.Equal(updatedOwner.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(updatedOwner.Email, result.Email);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, ownerSaveInDatabase.AccountId);
        Assert.Equal(ownerSaveInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(ownerSaveInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(ownerSaveInDatabase.Id, result.Id);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateOwner_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedOwner = new Owner()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com"
        };

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.UpdateAsync(requesterUser, 1, updatedOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Never);
    }

    [Theory]
    [InlineData("Name", null)]
    [InlineData("Name", "")]
    [InlineData("Name", " ")]
    [InlineData("Email", null)]
    [InlineData("Email", "")]
    [InlineData("Email", " ")]
    [InlineData("Password", null)]
    [InlineData("Password", "")]
    [InlineData("Password", " ")]
    [InlineData("DocumentId", null)]
    [InlineData("DocumentId", "")]
    [InlineData("DocumentId", " ")]
    public async Task UpdateOwner_TryToUpdateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedOwner = new Owner()
        {
            Name = "Name test 2",
            DocumentId = "My document 2",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
        };
        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        typeof(Owner).GetProperty(property).SetValue(updatedOwner, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => ownerService.UpdateAsync(requesterUser, 1, updatedOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Never);
    }

    [Fact]
    public async Task UpdateOwner_TryToUpdateWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
        };

        var ownerSaveInDatabase = new Owner()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Id = 1
        };

        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(ownerSaveInDatabase);
        _mockOwnerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Owner() { Id = 2 });

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.UpdateAsync(requesterUser, 1, updatedOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Once);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Never);
    }

    [Fact]
    public async Task UpdateOwner_TryToUpdateANonexistentOwnerAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
        };

        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Owner>(null);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.UpdateAsync(requesterUser, 1, updatedOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Never);
    }

    [Fact]
    public async Task UpdateOwner_TryToUpdateOwnerOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
        };

        var ownerSaveInDatabase = new Owner()
        {
            Name = "Name test",
            AccountId = 2,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Id = 1
        };

        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(ownerSaveInDatabase);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.UpdateAsync(requesterUser, 1, updatedOwner));
        _mockOwnerRepository.Verify(x => x.GetOneByEmail(updatedOwner.Email), Times.Never);
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(updatedOwner), Times.Never);
    }

    [Fact]
    public async Task DeleteOwner_SuccessfullyAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedOwner = new Owner()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
        };

        var ownerSaveInDatabase = new Owner()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Id = 1
        };

        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(ownerSaveInDatabase);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act
        await ownerService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(It.IsAny<Owner>()), Times.Once);
        _mockOwnerRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteOwner_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.DeleteAsync(requesterUser, 1));
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(It.IsAny<Owner>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteOwner_TryToDeleteANonexistentOwnerAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Owner>(null);

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.DeleteAsync(requesterUser, 1));
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(It.IsAny<Owner>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteOwner_TryToDeleteOwnerOfAnotherAccountAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockOwnerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Owner() { Id = 1, AccountId = 2 });

        var ownerService = new OwnerService(_mockOwnerRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => ownerService.DeleteAsync(requesterUser, 1));
        _mockOwnerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockOwnerRepository.Verify(x => x.UpdateAsync(It.IsAny<Owner>()), Times.Never);
        _mockOwnerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }
}
