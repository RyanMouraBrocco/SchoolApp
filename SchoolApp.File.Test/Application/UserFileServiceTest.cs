using Moq;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.File.Test.Application;

public class UserFileServiceTest
{
    private readonly Mock<IFileRepository<UserFile>> _mockFileRepository;

    public UserFileServiceTest()
    {
        _mockFileRepository = new Mock<IFileRepository<UserFile>>();
    }

    private string GetTypePathByUserType(UserTypeEnum userType)
    {
        return userType switch
        {
            UserTypeEnum.Manager => "manager",
            UserTypeEnum.Owner => "onwer",
            UserTypeEnum.Teacher => "teacher",
            _ => throw new NotImplementedException("Invalid user type")
        };
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task AddUserFile_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new UserFile() { Base64Value = "file", Extension = "jpg" };

        var fileService = new UserFileService(_mockFileRepository.Object);

        // Act
        await fileService.AddAsync(requesterUser, newFile);

        // Assert
        _mockFileRepository.Verify(x => x.AddAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, "Base64Value", "")]
    [InlineData(UserTypeEnum.Manager, "Extension", "")]
    [InlineData(UserTypeEnum.Manager, "Base64Value", " ")]
    [InlineData(UserTypeEnum.Manager, "Extension", " ")]
    [InlineData(UserTypeEnum.Manager, "Base64Value", null)]
    [InlineData(UserTypeEnum.Manager, "Extension", null)]
    [InlineData(UserTypeEnum.Teacher, "Base64Value", "")]
    [InlineData(UserTypeEnum.Teacher, "Extension", "")]
    [InlineData(UserTypeEnum.Teacher, "Base64Value", " ")]
    [InlineData(UserTypeEnum.Teacher, "Extension", " ")]
    [InlineData(UserTypeEnum.Teacher, "Base64Value", null)]
    [InlineData(UserTypeEnum.Teacher, "Extension", null)]
    [InlineData(UserTypeEnum.Owner, "Base64Value", "")]
    [InlineData(UserTypeEnum.Owner, "Extension", "")]
    [InlineData(UserTypeEnum.Owner, "Base64Value", " ")]
    [InlineData(UserTypeEnum.Owner, "Extension", " ")]
    [InlineData(UserTypeEnum.Owner, "Base64Value", null)]
    [InlineData(UserTypeEnum.Owner, "Extension", null)]
    public async Task AddUserFile_TryToAddWithNullOrEmptyTextsAsync(UserTypeEnum userType, string propertyName, string value)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new UserFile() { Base64Value = "file", Extension = "jpg" };

        var fileService = new UserFileService(_mockFileRepository.Object);

        typeof(UserFile).GetProperty(propertyName).SetValue(newFile, value);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockFileRepository.Verify(x => x.AddAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task RemoveUserFile_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new UserFile() { Base64Value = "file", Extension = "jpg", FileName = "file" };

        _mockFileRepository.Setup(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new UserFileService(_mockFileRepository.Object);

        // Act
        await fileService.RemoveAsync(requesterUser, newFile);

        // Assert
        _mockFileRepository.Verify(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    [InlineData(UserTypeEnum.Owner)]
    public async Task RemoveUserFile_TryToAddWithInvalidFileAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new UserFile() { Base64Value = "file", Extension = "jpg", FileName = "file" };

        _mockFileRepository.Setup(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>())).Returns(Task.FromResult(false));


        var fileService = new UserFileService(_mockFileRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockFileRepository.Verify(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, "")]
    [InlineData(UserTypeEnum.Manager, " ")]
    [InlineData(UserTypeEnum.Manager, null)]
    [InlineData(UserTypeEnum.Teacher, "")]
    [InlineData(UserTypeEnum.Teacher, " ")]
    [InlineData(UserTypeEnum.Teacher, null)]
    [InlineData(UserTypeEnum.Owner, "")]
    [InlineData(UserTypeEnum.Owner, " ")]
    [InlineData(UserTypeEnum.Owner, null)]
    public async Task RemoveUserFile_TryToAddWithInvalidFileNameTextAsync(UserTypeEnum userType, string fileName)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new UserFile() { Base64Value = "file", Extension = "jpg", FileName = fileName };

        _mockFileRepository.Setup(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new UserFileService(_mockFileRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockFileRepository.Verify(x => x.ExistsAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync($"users/{GetTypePathByUserType(requesterUser.Type)}/{requesterUser.UserId}/", It.IsAny<string>()), Times.Never);
    }
}
