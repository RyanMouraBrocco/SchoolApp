using System;
using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.IdentityProvider.Test.Application;

public class ManagerServiceTest
{
    private readonly Mock<IManagerRepository> _mockManagerRepository;
    private readonly Mock<IFunctionRepository> _mockFunctionRepository;

    public ManagerServiceTest()
    {
        _mockFunctionRepository = new Mock<IFunctionRepository>();
        _mockManagerRepository = new Mock<IManagerRepository>();

        _mockFunctionRepository.Setup(x => x.InsertAsync(It.IsAny<Function>())).Returns((Function x) => { x.Id = 1; return Task.FromResult(x); });
        _mockFunctionRepository.Setup(x => x.UpdateAsync(It.IsAny<Function>())).Returns((Function x) => { return Task.FromResult(x); });
        _mockFunctionRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));

        _mockManagerRepository.Setup(x => x.InsertAsync(It.IsAny<Manager>())).Returns((Manager x) => { x.Id = 1; return Task.FromResult(x); });
        _mockManagerRepository.Setup(x => x.UpdateAsync(It.IsAny<Manager>())).Returns((Manager x) => { return Task.FromResult(x); });
        _mockManagerRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateNewManager_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };
        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Function() { Name = "Fuc 1 ", AccountId = 1, Id = 1 });
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act
        var result = await managerService.CreateAsync(requesterUser, newManager);

        // Assert
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Once);
        Assert.Equal(newManager.Name, result.Name);
        Assert.Equal(newManager.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(newManager.Email, result.Email);
        Assert.Equal(newManager.Salary, result.Salary);
        Assert.Equal(newManager.FunctionId, result.FunctionId);
        Assert.Equal(newManager.HiringDate, result.HiringDate);
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
    public async Task CreateNewManager_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = purePassword,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
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
    public async Task CreateNewManager_TryToCreateWithoutASecurityPasswordAsync(string password)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = password,
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
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
    public async Task CreateNewManager_TryToCreateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };
        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        typeof(Manager).GetProperty(property).SetValue(newManager, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
    }

    [Fact]
    public async Task CreateNewManager_TryToCreateNegativeSalaryAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = -200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };
        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
    }

    [Fact]
    public async Task CreateNewManager_TryToWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Manager() { Id = 2 });

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
    }

    [Fact]
    public async Task CreateNewManager_TryToCreateWithNonexistentFunctionAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Function>(null);
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
    }

    [Fact]
    public async Task CreateNewManager_TryToCreateWithFunctionOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Function() { Name = "Fuc 1 ", AccountId = 2, Id = 1 });
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.CreateAsync(requesterUser, newManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(newManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(newManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.InsertAsync(newManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedManager = new Manager()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(1),
            FunctionId = 2
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText(purePassword),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Function() { Name = "Fuc 2", AccountId = 1, Id = 2 });
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);
        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act
        var result = await managerService.UpdateAsync(requesterUser, 1, updatedManager);

        // Assert
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Once);
        Assert.Equal(updatedManager.Name, result.Name);
        Assert.Equal(updatedManager.DocumentId, result.DocumentId);
        Assert.Equal(IdentityProvider.Application.Helpers.Utils.HashText(purePassword), result.Password);
        Assert.Equal(updatedManager.Email, result.Email);
        Assert.Equal(updatedManager.Salary, result.Salary);
        Assert.Equal(updatedManager.HiringDate, result.HiringDate);
        Assert.Equal(updatedManager.FunctionId, result.FunctionId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, managerSaveInDatabase.AccountId);
        Assert.Equal(managerSaveInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(managerSaveInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(managerSaveInDatabase.Id, result.Id);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateManager_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var purePassword = "1StrongP4ssw0rd@";
        var updatedManager = new Manager()
        {
            Name = "Name test 2",
            DocumentId = "My document 2 ",
            Password = purePassword + "2",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(1),
            FunctionId = 2
        };

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
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
    public async Task UpdateManager_TryToUpdateWithInvalidValuesAsync(string property, object invalidValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test 2",
            DocumentId = "My document 2",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 201.00M,
            HiringDate = DateTime.Now.AddDays(3),
            FunctionId = 2
        };
        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        typeof(Manager).GetProperty(property).SetValue(updatedManager, invalidValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateToNegativeSalaryAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e@e.com",
            Salary = -200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };
        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateWithDuplicatedEmailAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns(new Manager() { Id = 2 });

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateANonexistentManagerAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Manager>(null);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateManagerOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 2,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Never);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateWithNonexistentFunctionAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Function>(null);
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);
        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task UpdateManager_TryToUpdateWithFunctionOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Function() { Name = "Fuc 1 ", AccountId = 2, Id = 1 });
        _mockManagerRepository.Setup(x => x.GetOneByEmail(It.IsAny<string>())).Returns<Manager>(null);
        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.UpdateAsync(requesterUser, 1, updatedManager));
        _mockFunctionRepository.Verify(x => x.GetOneById(updatedManager.FunctionId), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneByEmail(updatedManager.Email), Times.Once);
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(updatedManager), Times.Never);
    }

    [Fact]
    public async Task DeleteManager_SuccessfullyAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var updatedManager = new Manager()
        {
            Name = "Name test",
            DocumentId = "My document",
            Password = "1StrongP4ssw0rd@",
            Email = "e2@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1
        };

        var managerSaveInDatabase = new Manager()
        {
            Name = "Name test",
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            DocumentId = "My document",
            Password = IdentityProvider.Application.Helpers.Utils.HashText("1StrongP4ssw0rd@"),
            Email = "e@e.com",
            Salary = 200.00M,
            HiringDate = DateTime.Now,
            FunctionId = 1,
            Id = 1
        };

        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(managerSaveInDatabase);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act
        await managerService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(It.IsAny<Manager>()), Times.Once);
        _mockManagerRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteManager_TryToAccessWithNoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.DeleteAsync(requesterUser, 1));
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Never);
        _mockManagerRepository.Verify(x => x.UpdateAsync(It.IsAny<Manager>()), Times.Never);
        _mockManagerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteManager_TryToDeleteANonexistentManagerAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Manager>(null);

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.DeleteAsync(requesterUser, 1));
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(It.IsAny<Manager>()), Times.Never);
        _mockManagerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

    [Fact]
    public async Task DeleteManager_TryToDeleteManagerOfAnotherAccountAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        _mockManagerRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(new Manager() { Id = 1, AccountId = 2 });

        var managerService = new ManagerService(_mockManagerRepository.Object, _mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => managerService.DeleteAsync(requesterUser, 1));
        _mockManagerRepository.Verify(x => x.GetOneById(1), Times.Once);
        _mockManagerRepository.Verify(x => x.UpdateAsync(It.IsAny<Manager>()), Times.Never);
        _mockManagerRepository.Verify(x => x.DeleteAsync(1), Times.Never);
    }

}
