using Moq;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.File.Test.Application;

public class AcitivityAnswerVersionFileServiceTest
{
    private readonly Mock<IActivityAnswerVersionRepository> _mockActivityAnswerVersionRepository;
    private readonly Mock<IFileRepository<ActivityAnswerVersionFile>> _mockFileRepository;
    private readonly Mock<IActivityRepository> _mockActivityRepository;
    private readonly Mock<IClassroomRepository> _mockClassromRepository;

    public AcitivityAnswerVersionFileServiceTest()
    {
        _mockActivityAnswerVersionRepository = new Mock<IActivityAnswerVersionRepository>();
        _mockFileRepository = new Mock<IFileRepository<ActivityAnswerVersionFile>>();
        _mockActivityRepository = new Mock<IActivityRepository>();
        _mockClassromRepository = new Mock<IClassroomRepository>();
    }

    [Fact]
    public async Task AddActivityAnswerVersionFile_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act
        await fileService.AddAsync(requesterUser, newFile);

        // Assert
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddActivityAnswerVersionFile_TryToAddWithInvalidUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddActivityAnswerVersionFile_TryToAddWithNonExistentActivityAnswerVersionAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityAnswerVersionDto>(null));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddActivityAnswerVersionFile_TryToAddWithNonExistentActivityAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityDto>(null));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddActivityAnswerVersionFile_TryToAddWithActivityOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 2, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddActivityAnswerVersionFile_TryToAddWithInvalidClassroomAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 2 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData("Base64Value", "")]
    [InlineData("Extension", "")]
    [InlineData("Base64Value", " ")]
    [InlineData("Extension", " ")]
    [InlineData("Base64Value", null)]
    [InlineData("Extension", null)]
    public async Task AddActivityAnswerVersionFile_TryToAddWithNullOrEmptyTextsAsync(string propertyName, string value)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        typeof(ActivityAnswerVersionFile).GetProperty(propertyName).SetValue(newFile, value);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("activityanswerversions/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1", FileName = "file" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act
        await fileService.RemoveAsync(requesterUser, newFile);

        // Assert
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithInvalidUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1", FileName = "file" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithInvalidActivityAnswerVersionAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1", FileName = "file" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityAnswerVersionDto>(null));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Never);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithInvalidFileAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1", FileName = "file" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(false));


        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithInvalidFileNameTextAsync(string fileName)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1", FileName = fileName };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));


        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithNonExistentActivityAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityDto>(null));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithActivityOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 2, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveActivityAnswerVersionFile_TryToAddWithInvalidClassroomAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new ActivityAnswerVersionFile() { Base64Value = "file", Extension = "jpg", ActivityAnswerVersionId = "1" };

        _mockActivityAnswerVersionRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityAnswerVersionDto() { Id = "1" }));
        _mockActivityRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new ActivityDto() { Id = "1", AccountId = 1, ClassroomId = 1 }));
        _mockClassromRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<ClassroomDto>>(new List<ClassroomDto>() { new() { Id = 2 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new ActivityAnswerVersionFileService(_mockFileRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityRepository.Object, _mockClassromRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockActivityAnswerVersionRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<string>()), Times.Once);
        _mockClassromRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("activityanswerversions/1/", It.IsAny<string>()), Times.Never);
    }

}
