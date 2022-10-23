using Moq;
using SchoolApp.Classroom.Application.Domain.Dtos;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Classroom.Application.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Classroom.Test.Application;

public class ActivityAnswerGradeServiceTest
{
    private readonly Mock<IActivityAnswerGradeRepository> _mockActivityAnswerGradeRepository;
    private readonly Mock<IStudentService> _mockStudentService;
    private readonly Mock<IActivityAnswerRepository> _mockActivityAnswerRepository;
    private readonly Mock<IClassroomService> _mockClassroomService;

    public ActivityAnswerGradeServiceTest()
    {
        _mockActivityAnswerGradeRepository = new Mock<IActivityAnswerGradeRepository>();
        _mockClassroomService = new Mock<IClassroomService>();
        _mockStudentService = new Mock<IStudentService>();
        _mockActivityAnswerRepository = new Mock<IActivityAnswerRepository>();

        _mockActivityAnswerGradeRepository.Setup(x => x.InsertAsync(It.IsAny<ActivityAnswerGrade>())).Returns((ActivityAnswerGrade x) => { x.Id = 1; return Task.FromResult(x); });
        _mockActivityAnswerGradeRepository.Setup(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>())).Returns((ActivityAnswerGrade x) => { return Task.FromResult(x); });
        _mockActivityAnswerGradeRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateActivityAnswerGrade_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 1,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act
        var result = await activityAnswerGradeService.CreateAsync(requesterUser, createGrade);

        // Assert
        Assert.Equal(createGrade.ActivityAnswerId, result.ActivityAnswerId);
        Assert.Equal(createGrade.StudentId, result.StudentId);
        Assert.Equal(createGrade.Value, result.Value);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateActivityAnswerGrade_TryToCreateWithOwnerAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 1,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.CreateAsync(requesterUser, createGrade));
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateActivityAnswerGrade_TryToCreateWithoutActivityAnswerAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityAnswerDto>(null));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 1,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.CreateAsync(requesterUser, createGrade));
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateActivityAnswerGrade_TryToCreateWithoutClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 1,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.CreateAsync(requesterUser, createGrade));
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateActivityAnswerGrade_TryToCreateWithoutStudentAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Students.Student>(null);

        var createGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 1,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.CreateAsync(requesterUser, createGrade));
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivityAnswerGrade_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act
        var result = await activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade);

        // Assert
        Assert.Equal(updateGrade.ActivityAnswerId, result.ActivityAnswerId);
        Assert.Equal(updateGrade.Value, result.Value);
        Assert.Equal(activityAnswerGradeInDatabase.StudentId, result.StudentId);
        Assert.Equal(activityAnswerGradeInDatabase.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(activityAnswerGradeInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(activityAnswerGradeInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(activityAnswerGradeInDatabase.Id, result.Id);
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.AtLeast(2));
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateActivityAnswerGrade_TryToCreateWithOwnerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivityAnswerGrade_TryToUpdateWithNonexistentGradeAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ActivityAnswerGrade>(null);
        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Never);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivityAnswerGrade_TryToUpdateGradeOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 2,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Never);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivityAnswerGrade_TryToUpdateWithANonExistentActivityAnswerAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityAnswerDto>(null));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivityAnswerGrade_TryToUpdateWithANonExistentClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var activityAnswer = new ActivityAnswerDto() { Id = "1234", Activity = new ActivityDto() { Id = "12345", ClassroomId = 1 } };
        _mockActivityAnswerRepository.Setup(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>())).Returns(Task.FromResult<ActivityAnswerDto>(activityAnswer));
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);

        var updateGrade = new ActivityAnswerGrade()
        {
            ActivityAnswerId = "1234",
            StudentId = 2,
            Value = 200
        };

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.GetOneByIdIncludingActivityAsync(It.IsAny<string>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivityAnswerGrade_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await activityAnswerGradeService.DeleteAsync(requesterUser, 1);
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>()), Times.Once);
        _mockActivityAnswerGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteActivityAnswerGrade_TryToDeleteWithOwnerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 1,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.DeleteAsync(requesterUser, 1));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockActivityAnswerGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>()), Times.Never);
        _mockActivityAnswerGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivityAnswerGrade_TryToDeleteWithNonExistentGradeAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ActivityAnswerGrade>(null);

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.DeleteAsync(requesterUser, 1));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>()), Times.Never);
        _mockActivityAnswerGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivityAnswerGrade_TryToDeleteGradeOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ActivityAnswerGrade()
        {
            Id = 1,
            AccountId = 2,
            ActivityAnswerId = "1234",
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockActivityAnswerGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var activityAnswerGradeService = new ActivityAnswerGradeService(_mockActivityAnswerGradeRepository.Object, _mockStudentService.Object, _mockActivityAnswerRepository.Object, _mockClassroomService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerGradeService.DeleteAsync(requesterUser, 1));
        _mockActivityAnswerGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockActivityAnswerGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ActivityAnswerGrade>()), Times.Never);
        _mockActivityAnswerGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

}
