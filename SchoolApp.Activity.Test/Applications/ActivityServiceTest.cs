using Moq;
using SchoolApp.Activity.Application.Domain.Dtos;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Activity.Test.Applications;

public class ActivityServiceTest
{
    private readonly Mock<IActivityRepository> _mockActivityRepository;
    private readonly Mock<IClassroomRepository> _mockClassroomRepository;

    public ActivityServiceTest()
    {
        _mockActivityRepository = new Mock<IActivityRepository>();
        _mockClassroomRepository = new Mock<IClassroomRepository>();

        _mockActivityRepository.Setup(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>())).Returns((Activity.Application.Domain.Entities.Activities.Activity x) => { x.Id = "1"; return Task.FromResult(x); });
        _mockActivityRepository.Setup(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>())).Returns((Activity.Application.Domain.Entities.Activities.Activity x) => { return Task.FromResult(x); });
        _mockActivityRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()));
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewActivity_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act
        var result = await activityService.CreateAsync(requesterUser, newActivity);

        // Assert
        Assert.Equal(newActivity.ClassroomId, result.ClassroomId);
        Assert.Equal(newActivity.Name, result.Name);
        Assert.Equal(newActivity.Description, result.Description);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.CreatorId);
        Assert.Null(result.UpdateDate);
        Assert.Null(result.UpdaterId);
        Assert.NotNull(result.Id);
        _mockActivityRepository.Verify(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Once);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateNewActivity_TryToCreateWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.CreateAsync(requesterUser, newActivity));
        _mockActivityRepository.Verify(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, "Description", " ")]
    [InlineData(UserTypeEnum.Manager, "Description", "")]
    [InlineData(UserTypeEnum.Manager, "Description", null)]
    [InlineData(UserTypeEnum.Manager, "Name", " ")]
    [InlineData(UserTypeEnum.Manager, "Name", "")]
    [InlineData(UserTypeEnum.Manager, "Name", null)]
    [InlineData(UserTypeEnum.Teacher, "Description", " ")]
    [InlineData(UserTypeEnum.Teacher, "Description", "")]
    [InlineData(UserTypeEnum.Teacher, "Description", null)]
    [InlineData(UserTypeEnum.Teacher, "Name", " ")]
    [InlineData(UserTypeEnum.Teacher, "Name", "")]
    [InlineData(UserTypeEnum.Teacher, "Name", null)]
    public async Task CreateNewActivity_TryToCreateWithEmptyAndNullStringsAsync(UserTypeEnum userType, string propertyName, string propertyValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new ClassroomDto() { Id = 1, AccountId = 1, TeacherId = 1 }));

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        newActivity.GetType().GetProperty(propertyName).SetValue(newActivity, propertyValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => activityService.CreateAsync(requesterUser, newActivity));
        _mockActivityRepository.Verify(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewActivity_TryToCreateWithNonExistentClassroomAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<ClassroomDto>(null));

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.CreateAsync(requesterUser, newActivity));
        _mockActivityRepository.Verify(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateNewActivity_TryToCreateWithClassroomOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockClassroomRepository.Setup(x => x.GetOneByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<ClassroomDto>(new ClassroomDto() { Id = 1, AccountId = 2 }));

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.CreateAsync(requesterUser, newActivity));
        _mockActivityRepository.Verify(x => x.InsertAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockClassroomRepository.Verify(x => x.GetOneByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivity_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Name = "Name 1",
            Description = "Description 1"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act
        var result = await activityService.UpdateAsync(requesterUser, "1234", newActivity);

        // Assert
        Assert.Equal(activityInDatabase.ClassroomId, result.ClassroomId);
        Assert.Equal(newActivity.Name, result.Name);
        Assert.Equal(newActivity.Description, result.Description);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(result.CreationDate, result.CreationDate);
        Assert.Equal(activityInDatabase.CreatorId, result.CreatorId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.UpdateDate.Value.ToString("MM/dd/yyyy HH:mm"));
        Assert.Equal(requesterUser.UserId, result.UpdaterId);
        Assert.Equal(activityInDatabase.Id, result.Id);
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateActivity_TryToUpdateWithInvalidUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Name = "Name 1",
            Description = "Description 1"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);


        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.UpdateAsync(requesterUser, "1234", newActivity));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Never);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivity_TryToUpdateWithNonExistentActivityAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns<Activity.Application.Domain.Entities.Activities.Activity>(null);

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Name = "Name 1",
            Description = "Description 1"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);


        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.UpdateAsync(requesterUser, "1234", newActivity));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task UpdateActivity_TryToUpdateWithActivityOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 2,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Name = "Name 1",
            Description = "Description 1"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.UpdateAsync(requesterUser, "1234", newActivity));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager, "Description", " ")]
    [InlineData(UserTypeEnum.Manager, "Description", "")]
    [InlineData(UserTypeEnum.Manager, "Description", null)]
    [InlineData(UserTypeEnum.Manager, "Name", " ")]
    [InlineData(UserTypeEnum.Manager, "Name", "")]
    [InlineData(UserTypeEnum.Manager, "Name", null)]
    [InlineData(UserTypeEnum.Teacher, "Description", " ")]
    [InlineData(UserTypeEnum.Teacher, "Description", "")]
    [InlineData(UserTypeEnum.Teacher, "Description", null)]
    [InlineData(UserTypeEnum.Teacher, "Name", " ")]
    [InlineData(UserTypeEnum.Teacher, "Name", "")]
    [InlineData(UserTypeEnum.Teacher, "Name", null)]
    public async Task UpdateActivity_TryToUpdateWithEmptyAndNullStringsAsync(UserTypeEnum userType, string propertyName, string propertyValue)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var newActivity = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            ClassroomId = 1,
            Name = "name",
            Description = "description"
        };

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        newActivity.GetType().GetProperty(propertyName).SetValue(newActivity, propertyValue);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => activityService.UpdateAsync(requesterUser, "1234", newActivity));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivity_SucessfullyAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act
        await activityService.DeleteAsync(requesterUser, "1234");

        // Assert
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Once);
        _mockActivityRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteActivity_TryToDeleteWithNoManagerUserAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 1,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.DeleteAsync(requesterUser, "1234"));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Never);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockActivityRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivity_TryToDeleteWithNonExistentActivityAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns<Activity.Application.Domain.Entities.Activities.Activity>(null);

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.DeleteAsync(requesterUser, "1234"));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockActivityRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task DeleteActivity_TryToDeleteWithActivityOfAnotherAccountAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var activityInDatabase = new Activity.Application.Domain.Entities.Activities.Activity()
        {
            Id = "1234",
            AccountId = 2,
            CreationDate = DateTime.Now.AddDays(-3),
            CreatorId = 1,
            Description = "Description",
            Name = "Name",
            ClassroomId = 1
        };

        _mockActivityRepository.Setup(x => x.GetOneById(It.IsAny<string>())).Returns(activityInDatabase);

        var activityService = new ActivityService(_mockActivityRepository.Object, _mockClassroomRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityService.DeleteAsync(requesterUser, "1234"));
        _mockActivityRepository.Verify(x => x.GetOneById(It.IsAny<string>()), Times.Once);
        _mockActivityRepository.Verify(x => x.UpdateAsync(It.IsAny<Activity.Application.Domain.Entities.Activities.Activity>()), Times.Never);
        _mockActivityRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

}
