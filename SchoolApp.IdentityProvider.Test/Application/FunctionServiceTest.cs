using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Services;
using SchoolApp.IdentityProvider.Sql.Context;
using SchoolApp.IdentityProvider.Sql.Dtos.Functions;
using SchoolApp.IdentityProvider.Sql.Repositories;
using SchoolApp.IdentityProvider.Test.Utils;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.IdentityProvider.Test.Application;

public class FunctionServiceTest
{
    private readonly Mock<IFunctionRepository> _mockFunctionRepository;

    public FunctionServiceTest()
    {
        _mockFunctionRepository = new Mock<IFunctionRepository>();
        _mockFunctionRepository.Setup(x => x.InsertAsync(It.IsAny<Function>())).Returns((Function x) => { x.Id = 1; return Task.FromResult(x); });
        _mockFunctionRepository.Setup(x => x.UpdateAsync(It.IsAny<Function>())).Returns((Function x) => { return Task.FromResult(x); });
    }


    [Fact]
    public async Task CreateNewFunction_ManagerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newFunction = new Function() { Name = "Name test", Description = "Description test" };

        var functionService = new FunctionService(_mockFunctionRepository.Object);

        // Act
        var result = await functionService.CreateAsync(requesterUser, newFunction);

        // Assert
        _mockFunctionRepository.Verify(x => x.InsertAsync(newFunction), Times.Once);
        Assert.Equal(newFunction.Name, result.Name);
        Assert.Equal(newFunction.Description, result.Description);
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
    public async Task CreateNewFunction_NoManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFunction = new Function() { Name = "Name test", Description = "Description test" };
        var functionService = new FunctionService(_mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => functionService.CreateAsync(requesterUser, newFunction));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateNewFunction_TryToCreateInvalidNameAsync(string name)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var newFunction = new Function() { Name = name, Description = "Description test" };
        var functionService = new FunctionService(_mockFunctionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => functionService.CreateAsync(requesterUser, newFunction));
    }

    [Fact]
    public async Task UpdateFunction_ManagerUserAsync()
    {
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);
        var fakeFuctionId = 1;
        var updatedFunction = new Function()
        {
            Name = "Name test",
            Description = "Description test"
        };
        var functionService = new FunctionService(_mockFunctionRepository.Object);
        var functionSavedInDatabase = new Function()
        {
            Id = fakeFuctionId,
            Name = "Old name test",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = null
        };
        _mockFunctionRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(functionSavedInDatabase);

        // Act
        var result = await functionService.UpdateAsync(requesterUser, fakeFuctionId, updatedFunction);

        // Assert
        _mockFunctionRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockFunctionRepository.Verify(x => x.UpdateAsync(It.IsAny<Function>()), Times.Once);
        Assert.Equal(fakeFuctionId, result.Id);
        Assert.Equal(updatedFunction.Name, result.Name);
        Assert.Equal(updatedFunction.Description, result.Description);
        Assert.Equal(functionSavedInDatabase.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(functionSavedInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(functionSavedInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
    }

}