using Moq;
using SchoolApp.Classroom.Application.Domain.Entities.Classrooms;
using SchoolApp.Classroom.Application.Domain.Entities.Grades;
using SchoolApp.Classroom.Application.Interfaces.Repositories;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Classroom.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Classroom.Test.Application;

public class ClassroomStudentGradeServiceTest
{
    private readonly Mock<IClassroomStudentGradeRepository> _mockClassroomStudentGradeRepository;
    private readonly Mock<IStudentService> _mockStudentService;
    private readonly Mock<IClassroomStudentRepository> _mockClassroomStudentRepository;
    private readonly Mock<IClassroomService> _mockClassroomService;

    public ClassroomStudentGradeServiceTest()
    {
        _mockClassroomStudentGradeRepository = new Mock<IClassroomStudentGradeRepository>();
        _mockClassroomService = new Mock<IClassroomService>();
        _mockStudentService = new Mock<IStudentService>();
        _mockClassroomStudentRepository = new Mock<IClassroomStudentRepository>();

        _mockClassroomStudentGradeRepository.Setup(x => x.InsertAsync(It.IsAny<ClassroomStudentGrade>())).Returns((ClassroomStudentGrade x) => { x.Id = 1; return Task.FromResult(x); });
        _mockClassroomStudentGradeRepository.Setup(x => x.UpdateAsync(It.IsAny<ClassroomStudentGrade>())).Returns((ClassroomStudentGrade x) => { return Task.FromResult(x); });
        _mockClassroomStudentGradeRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateClassroomStudentGrade_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 1,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act
        var result = await classroomStudentGradeService.CreateAsync(requesterUser, createGrade);

        // Assert
        Assert.Equal(createGrade.ClassroomStudentId, result.ClassroomStudentId);
        Assert.Equal(createGrade.StudentId, result.StudentId);
        Assert.Equal(createGrade.Value, result.Value);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.True(result.Id > 0);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateClassroomStudentGrade_TryToCreateWithOwnerAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 1,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.CreateAsync(requesterUser, createGrade));
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateClassroomStudentGrade_TryToCreateWithoutActivityAnswerAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ClassroomStudent>(null);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 1,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.CreateAsync(requesterUser, createGrade));
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateClassroomStudentGrade_TryToCreateWithoutClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Students.Student() { Id = 1, AccountId = 1, Name = "Student" });

        var createGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 1,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.CreateAsync(requesterUser, createGrade));
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateClassroomStudentGrade_TryToCreateWithoutStudentAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });
        _mockStudentService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Students.Student>(null);

        var createGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 1,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.CreateAsync(requesterUser, createGrade));
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
        _mockStudentService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroomStudentGrade_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 2,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var classroomStudentInDatabase = new ClassroomStudent() { Id = 2, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act
        var result = await classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade);

        // Assert
        Assert.Equal(updateGrade.ClassroomStudentId, result.ClassroomStudentId);
        Assert.Equal(updateGrade.Value, result.Value);
        Assert.Equal(activityAnswerGradeInDatabase.StudentId, result.StudentId);
        Assert.Equal(activityAnswerGradeInDatabase.AccountId, result.AccountId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(activityAnswerGradeInDatabase.CreationDate, result.CreationDate);
        Assert.Equal(activityAnswerGradeInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(activityAnswerGradeInDatabase.Id, result.Id);
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.AtLeast(2));
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateClassroomStudentGrade_TryToCreateWithOwnerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroomStudentGrade_TryToUpdateWithNonexistentGradeAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ClassroomStudentGrade>(null);
        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroomStudentGrade_TryToUpdateGradeOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 2,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroomStudentGrade_TryToUpdateWithANonExistentActivityAnswerAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ClassroomStudent>(null);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns(new Classroom.Application.Domain.Entities.Classrooms.Classroom() { AccountId = 1, Id = 1 });

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateClassroomStudentGrade_TryToUpdateWithANonExistentClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);
        var classroomStudentInDatabase = new ClassroomStudent() { Id = 1, StudentId = 1, ClassroomId = 1 };
        _mockClassroomStudentRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(classroomStudentInDatabase);
        _mockClassroomService.Setup(x => x.GetOneById(requesterUser, It.IsAny<int>())).Returns<Classroom.Application.Domain.Entities.Classrooms.Classroom>(null);

        var updateGrade = new ClassroomStudentGrade()
        {
            ClassroomStudentId = 1,
            StudentId = 2,
            Value = 200
        };

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.UpdateAsync(requesterUser, 1, updateGrade));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomService.Verify(x => x.GetOneById(requesterUser, It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteClassroomStudentGrade_SuccessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await classroomStudentGradeService.DeleteAsync(requesterUser, 1);
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ClassroomStudentGrade>()), Times.Once);
        _mockClassroomStudentGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteClassroomStudentGrade_TryToDeleteWithOwnerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 1,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.DeleteAsync(requesterUser, 1));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Never);
        _mockClassroomStudentGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ClassroomStudentGrade>()), Times.Never);
        _mockClassroomStudentGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteClassroomStudentGrade_TryToDeleteWithNonExistentGradeAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);
        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns<ClassroomStudentGrade>(null);

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.DeleteAsync(requesterUser, 1));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ClassroomStudentGrade>()), Times.Never);
        _mockClassroomStudentGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteClassroomStudentGrade_TryToDeleteGradeOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityAnswerGradeInDatabase = new ClassroomStudentGrade()
        {
            Id = 1,
            AccountId = 2,
            ClassroomStudentId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            StudentId = 1
        };

        _mockClassroomStudentGradeRepository.Setup(x => x.GetOneById(It.IsAny<int>())).Returns(activityAnswerGradeInDatabase);

        var classroomStudentGradeService = new ClassroomStudentGradeService(_mockClassroomStudentGradeRepository.Object, _mockStudentService.Object, _mockClassroomService.Object, _mockClassroomStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => classroomStudentGradeService.DeleteAsync(requesterUser, 1));
        _mockClassroomStudentGradeRepository.Verify(x => x.GetOneById(It.IsAny<int>()), Times.Once);
        _mockClassroomStudentGradeRepository.Verify(x => x.UpdateAsync(It.IsAny<ClassroomStudentGrade>()), Times.Never);
        _mockClassroomStudentGradeRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
