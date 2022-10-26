using Moq;
using SchoolApp.Classroom.Application.Domain.Entities.Students;
using SchoolApp.Classroom.Application.Domain.Enums;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Classroom.Test.Application;

public class StudentServiceTest
{
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    public StudentServiceTest()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();

        _mockStudentRepository.Setup(x => x.InsertAsync(It.IsAny<Student>())).Returns((Student x) => { x.Id = 1; return Task.FromResult(x); });
        _mockStudentRepository.Setup(x => x.UpdateAsync(It.IsAny<Student>())).Returns((Student x) => { return Task.FromResult(x); });
        _mockStudentRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateStudent_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act
        var result = await studentService.CreateAsync(requesterUser, newStudent);

        // Assert
        Assert.Equal(newStudent.Name, result.Name);
        Assert.Equal(newStudent.DocumentId, result.DocumentId);
        Assert.Equal(newStudent.BirthDate, result.BirthDate);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
        _mockStudentRepository.Verify(x => x.InsertAsync(It.IsAny<Student>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateStudent_TryToCreateWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.CreateAsync(requesterUser, newStudent));
        _mockStudentRepository.Verify(x => x.InsertAsync(It.IsAny<Student>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task CreateStudent_TryToCreateWithNullOrEmptyNameAsync(string name)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var newStudent = new Student()
        {
            Name = name,
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => studentService.CreateAsync(requesterUser, newStudent));
        _mockStudentRepository.Verify(x => x.InsertAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task CreateStudent_TryToCreateWithNotValidSexAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = (SexTypeEnum)44
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => studentService.CreateAsync(requesterUser, newStudent));
        _mockStudentRepository.Verify(x => x.InsertAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStudent_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act
        var result = await studentService.UpdateAsync(requesterUser, 1, newStudent);

        // Assert
        Assert.Equal(newStudent.Name, result.Name);
        Assert.Equal(newStudent.DocumentId, result.DocumentId);
        Assert.Equal(newStudent.BirthDate, result.BirthDate);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(studentInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(studentInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(studentInDatabase.Id, result.Id);
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateStudent_TryToUpadateWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.UpdateAsync(requesterUser, 1, newStudent));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.InsertAsync(It.IsAny<Student>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task UpdateStudent_TryToUpdateWithNullOrEmptyNameAsync(string name)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var newStudent = new Student()
        {
            Name = name,
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Male
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => studentService.UpdateAsync(requesterUser, 1, newStudent));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStudent_TryToUpdateWithNotValidSexAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = (SexTypeEnum)44
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => studentService.UpdateAsync(requesterUser, 1, newStudent));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStudent_TryToUpdateNonExistentStudentAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Student>(null);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Female
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.UpdateAsync(requesterUser, 1, newStudent));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStudent_TryToUpdatStudentOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 2,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var newStudent = new Student()
        {
            Name = "Student",
            DocumentId = "1234",
            BirthDate = DateTime.Now.AddYears(-5),
            Sex = SexTypeEnum.Female
        };

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.UpdateAsync(requesterUser, 1, newStudent));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task DeleteStudent_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act
        await studentService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Once);
        _mockStudentRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteStudent_TryToDeleteWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 1,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.DeleteAsync(requesterUser, 1));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
        _mockStudentRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteStudent_TryToDeleteNonExistentStudentAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Student>(null);

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.DeleteAsync(requesterUser, 1));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
        _mockStudentRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteStudent_TryToDeleteStudentOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var studentInDatabase = new Student()
        {
            Id = 1,
            AccountId = 2,
            BirthDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            DocumentId = "123",
            Name = "Studen",
            Sex = SexTypeEnum.Female
        };

        _mockStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(studentInDatabase);

        var studentService = new StudentService(_mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => studentService.DeleteAsync(requesterUser, 1));
        _mockStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockStudentRepository.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
        _mockStudentRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
