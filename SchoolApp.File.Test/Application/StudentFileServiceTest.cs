using Moq;
using SchoolApp.File.Application.Domain.Dtos;
using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.File.Test.Application;

public class StudentFileServiceTest
{
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<IFileRepository<StudentFile>> _mockFileRepository;

    public StudentFileServiceTest()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockFileRepository = new Mock<IFileRepository<StudentFile>>();
    }

    [Fact]
    public async Task AddStudentFile_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1 };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act
        await fileService.AddAsync(requesterUser, newFile);

        // Assert
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("students/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task AddStudentFile_TryToAddWithInvalidUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1 };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.AddAsync("students/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task AddStudentFile_TryToAddWithInvalidStudentAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1 };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 2 } }));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("students/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Theory]
    [InlineData("Base64Value", "")]
    [InlineData("Extension", "")]
    [InlineData("Base64Value", " ")]
    [InlineData("Extension", " ")]
    [InlineData("Base64Value", null)]
    [InlineData("Extension", null)]
    public async Task AddStudentFile_TryToAddWithNullOrEmptyTextsAsync(string propertyName, string value)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1 };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        typeof(StudentFile).GetProperty(propertyName).SetValue(newFile, value);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.AddAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.AddAsync("students/1/", It.IsAny<string>(), It.IsAny<MemoryStream>()), Times.Never);
    }

    [Fact]
    public async Task RemoveStudentFile_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1, FileName = "filename" };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("students/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act
        await fileService.RemoveAsync(requesterUser, newFile);

        // Assert
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("students/1/", It.IsAny<string>()), Times.Once);
        _mockFileRepository.Verify(x => x.ExistsAsync("students/1/", It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task RemoveStudentFile_TryToAddWithInvalidUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1, FileName = "filename" };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("students/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Never);
        _mockFileRepository.Verify(x => x.DeleteAsync("students/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("students/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveStudentFile_TryToAddWithInvalidStudentAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1, FileName = "filename" };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 2 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("students/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("students/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("students/1/", It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RemoveStudentFile_TryToAddWithInvalidFileAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1, FileName = "filename" };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("students/1/", It.IsAny<string>())).Returns(Task.FromResult(false));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("students/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("students/1/", It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task RemoveStudentFile_TryToAddWithInvalidFileNameTextAsync(string fileName)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);
        var newFile = new StudentFile() { Base64Value = "file", Extension = "jpg", StudentId = 1, FileName = fileName };

        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(It.IsAny<int>())).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));
        _mockFileRepository.Setup(x => x.ExistsAsync("students/1/", It.IsAny<string>())).Returns(Task.FromResult(true));

        var fileService = new StudentFileService(_mockFileRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => fileService.RemoveAsync(requesterUser, newFile));
        _mockStudentRepository.Verify(x => x.GetAllByOwnerIdAsync(It.IsAny<int>()), Times.Once);
        _mockFileRepository.Verify(x => x.DeleteAsync("students/1/", It.IsAny<string>()), Times.Never);
        _mockFileRepository.Verify(x => x.ExistsAsync("students/1/", It.IsAny<string>()), Times.Never);
    }
}
