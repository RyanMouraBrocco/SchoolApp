using Moq;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.File.Test.Application;

public class ActivityFileServiceTest
{
    private readonly Mock<IFileRepository<ActivityFile>> _mockFileRepository;
    private readonly Mock<IActivityRepository> _mockActivityRepository;
    private readonly Mock<IClassroomRepository> _mockClassromRepository;

    public ActivityFileServiceTest()
    {
        _mockFileRepository = new Mock<IFileRepository<ActivityFile>>();
        _mockActivityRepository = new Mock<IActivityRepository>();
        _mockClassromRepository = new Mock<IClassroomRepository>();
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityFile_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act
        await fileService.AddAsync(requesterUser, newFile);

        // Assert
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Once);
    }

    [Fact]
    public async Task AddActivityFile_TryToAddWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityFile_TryToAddWithNonExistentActivityAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityDto>(null));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityFile_TryToAddWithActivityOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 2, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityFile_TryToAddWithNonExistentClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<ClassroomDto>(null));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityFile_TryToAddWithClassroomOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 2, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddActivityFile_TryToAddWithClassroomOfAnotherTeacherAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Teacher);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 2 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
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
    public async Task AddActivityFile_TryToAddWithNullOrEmptyTextsAsync(UserTypeEnum userType, string propertyName, string value)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        typeof(ActivityFile).GetProperty(propertyName).SetValue(newFile, value);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activities/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1", FileName = "file" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act
        await fileService.RemoveAsync(requesterUser, newFile);

        // Assert
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RemoveActivityFile_TryToAddWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1", FileName = "file" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_TryToAddWithInvalidFileAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1", FileName = "file" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(false));


        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, "")]
    [InlineData(UserTypeEnum.Manager, " ")]
    [InlineData(UserTypeEnum.Manager, null)]
    [InlineData(UserTypeEnum.Teacher, "")]
    [InlineData(UserTypeEnum.Teacher, " ")]
    [InlineData(UserTypeEnum.Teacher, null)]
    public async Task RemoveActivityFile_TryToAddWithInvalidFileNameTextAsync(UserTypeEnum userType, string fileName)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1", FileName = fileName };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_TryToAddWithNonExistentActivityAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityDto>(null));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_TryToAddWithActivityOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 2, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_TryToAddWithNonExistentClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<ClassroomDto>(null));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityFile_TryToAddWithClassroomOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 2, TeacherId = 1 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityFile_TryToAddWithClassroomOfAnotherTeacherAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Teacher);
        var newFile = new ActivityFile() { Base64Value = "file", Extension = "jpg", ActivityId = "1" };

        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 2 }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activities/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityFileService(_mockFileRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activities/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activities/1/", It.IsAny<string>()), Times.Never);
    }

}
