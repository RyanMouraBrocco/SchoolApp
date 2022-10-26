using Moq;
using SchoolApp.Classroom.Application.Domain.Entities.Subjects;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Classroom.Test.Application;

public class SubjectServiceTest
{
    private readonly Mock<ISubjectRepository> _mockSubjectRepository;
    public SubjectServiceTest()
    {
        _mockSubjectRepository = new Mock<ISubjectRepository>();

        _mockSubjectRepository.Setup(x => x.InsertAsync(It.IsAny<Subject>())).Returns((Subject x) => { x.Id = 1; return Task.FromResult(x); });
        _mockSubjectRepository.Setup(x => x.UpdateAsync(It.IsAny<Subject>())).Returns((Subject x) => { return Task.FromResult(x); });
        _mockSubjectRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Fact]
    public async Task CreateSubject_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var newSubject = new Subject()
        {
            Name = "Subject"
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act
        var result = await subjectService.CreateAsync(requesterUser, newSubject);

        // Assert
        Assert.Equal(newSubject.Name, result.Name);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
        _mockSubjectRepository.Verify(x => x.InsertAsync(It.IsAny<Subject>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateSubject_TryToCreateWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newSubject = new Subject()
        {
            Name = "Subject",
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.CreateAsync(requesterUser, newSubject));
        _mockSubjectRepository.Verify(x => x.InsertAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task CreateSubject_TryToCreateWithNullOrEmptyNameAsync(string name)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var newSubject = new Subject()
        {
            Name = name,
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => subjectService.CreateAsync(requesterUser, newSubject));
        _mockSubjectRepository.Verify(x => x.InsertAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSubject_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var newSubject = new Subject()
        {
            Name = "Subject"
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act
        var result = await subjectService.UpdateAsync(requesterUser, 1, newSubject);

        // Assert
        Assert.Equal(newSubject.Name, result.Name);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(subjectInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(subjectInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(subjectInDatabase.Id, result.Id);
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateSubject_TryToUpadateWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var newSubject = new Subject()
        {
            Name = "Subject",
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.UpdateAsync(requesterUser, 1, newSubject));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.InsertAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task UpdateSubject_TryToUpdateWithNullOrEmptyNameAsync(string name)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var newSubject = new Subject()
        {
            Name = name,
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => subjectService.UpdateAsync(requesterUser, 1, newSubject));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSubject_TryToUpdateNonExistentSubjectAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Subject>(null);

        var newSubject = new Subject()
        {
            Name = "Subject",
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.UpdateAsync(requesterUser, 1, newSubject));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSubject_TryToUpdatSubjectOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 2,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var newSubject = new Subject()
        {
            Name = "Subject",
        };

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.UpdateAsync(requesterUser, 1, newSubject));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSubject_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act
        await subjectService.DeleteAsync(requesterUser, 1);

        // Assert
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Owner)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteSubject_TryToDeleteWithNonManagerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 1,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.DeleteAsync(requesterUser, 1));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSubject_TryToDeleteNonExistentSubjectAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<Subject>(null);

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.DeleteAsync(requesterUser, 1));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSubject_TryToDeleteSubjectOfAnotherAccountAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Manager);

        var subjectInDatabase = new Subject()
        {
            Id = 1,
            AccountId = 2,
            CreatorId = 1,
            CreationDate = DateTime.Now.AddDays(-1),
            Name = "Studen",
        };

        _mockSubjectRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(subjectInDatabase);

        var subjectService = new SubjectService(_mockSubjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => subjectService.DeleteAsync(requesterUser, 1));
        _mockSubjectRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockSubjectRepository.Verify(x => x.UpdateAsync(It.IsAny<Subject>()), Times.Never);
        _mockSubjectRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
