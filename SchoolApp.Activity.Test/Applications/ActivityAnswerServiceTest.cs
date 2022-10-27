using Moq;
using SchoolApp.Activity.Application.Domain.Dtos;
using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Activity.Application.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Test.Helpers;

namespace SchoolApp.Activity.Test.Applications;

public class ActivityAnswerServiceTest
{
    private readonly Mock<IActivityAnswerRepository> _mockActivityAnswerRepository;
    private readonly Mock<IActivityAnswerVersionRepository> _mockActivityAnswerVersionRepository;
    private readonly Mock<IActivityService> _mockActivityService;
    private readonly Mock<IActivityRepository> _mockActivityRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;

    public ActivityAnswerServiceTest()
    {
        _mockActivityAnswerRepository = new Mock<IActivityAnswerRepository>();
        _mockActivityAnswerVersionRepository = new Mock<IActivityAnswerVersionRepository>();
        _mockActivityService = new Mock<IActivityService>();
        _mockActivityRepository = new Mock<IActivityRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();

        _mockActivityAnswerRepository.Setup(x => x.InsertAsync(It.IsAny<ActivityAnswer>())).Returns((ActivityAnswer x) => { x.Id = "1234"; return Task.FromResult(x); });
        _mockActivityAnswerRepository.Setup(x => x.UpdateAsync(It.IsAny<ActivityAnswer>())).Returns((ActivityAnswer x) => { return Task.FromResult(x); });
        _mockActivityAnswerRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()));
        _mockActivityAnswerVersionRepository.Setup(x => x.InsertAsync(It.IsAny<ActivityAnswerVersion>())).Returns((ActivityAnswerVersion x) => { x.Id = "1234_v"; return Task.FromResult(x); });
        _mockActivityAnswerVersionRepository.Setup(x => x.UpdateAsync(It.IsAny<ActivityAnswerVersion>())).Returns((ActivityAnswerVersion x) => { return Task.FromResult(x); });
        _mockActivityAnswerVersionRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task CreateActivityAnswer_SuccessfullyAsync()
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(UserTypeEnum.Owner);

        var newActivityAnswer = new ActivityAnswer()
        {
            ActivityId = "1234",
            StudentId = 1,
            LastReview = new ActivityAnswerVersion()
            {
                Text = "new answer"
            }
        };

        _mockActivityService.Setup(x => x.GetOneByIdAsync(requesterUser, It.IsAny<string>())).Returns(Task.FromResult(new Application.Domain.Entities.Activities.Activity() { Id = "1", AccountId = 1, Closed = false }));
        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(requesterUser.UserId)).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));

        var activityAnswerService = new ActivityAnswerService(_mockActivityAnswerRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityService.Object, _mockActivityRepository.Object, _mockStudentRepository.Object);

        // Act
        var result = await activityAnswerService.CreateAsync(requesterUser, newActivityAnswer);

        // Assert
        Assert.Equal(newActivityAnswer.ActivityId, result.ActivityId);
        Assert.Equal(newActivityAnswer.StudentId, result.StudentId);
        Assert.Equal(requesterUser.AccountId, result.AccountId);
        Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), result.CreationDate.ToString("MM/dd/yyyy HH:mm"));
        _mockActivityService.Verify(x => x.GetOneByIdAsync(requesterUser, It.IsAny<string>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.InsertAsync(It.IsAny<ActivityAnswer>()), Times.Once);
        _mockActivityAnswerVersionRepository.Verify(x => x.InsertAsync(It.IsAny<ActivityAnswerVersion>()), Times.Once);
        _mockActivityAnswerRepository.Verify(x => x.SetLastReview(It.IsAny<string>(), It.IsAny<ActivityAnswerVersion>()), Times.Once);
    }

    [Theory]
    [InlineData(UserTypeEnum.Manager)]
    [InlineData(UserTypeEnum.Teacher)]
    public async Task CreateActivityAnswer_TryToCreateWithNonOwnerUserAsync(UserTypeEnum userType)
    {
        // Arrange
        var requesterUser = Helper.CreateRequesterUser1(userType);

        var newActivityAnswer = new ActivityAnswer()
        {
            ActivityId = "1234",
            StudentId = 1,
            LastReview = new ActivityAnswerVersion()
            {
                Text = "new answer"
            }
        };

        _mockActivityService.Setup(x => x.GetOneByIdAsync(requesterUser, It.IsAny<string>())).Returns(Task.FromResult(new Application.Domain.Entities.Activities.Activity() { Id = "1", AccountId = 1, Closed = false }));
        _mockStudentRepository.Setup(x => x.GetAllByOwnerIdAsync(requesterUser.UserId)).Returns(Task.FromResult<IList<StudentDto>>(new List<StudentDto>() { new() { Id = 1 } }));

        var activityAnswerService = new ActivityAnswerService(_mockActivityAnswerRepository.Object, _mockActivityAnswerVersionRepository.Object, _mockActivityService.Object, _mockActivityRepository.Object, _mockStudentRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => activityAnswerService.CreateAsync(requesterUser, newActivityAnswer));
        _mockActivityService.Verify(x => x.GetOneByIdAsync(requesterUser, It.IsAny<string>()), Times.Never);
        _mockActivityAnswerRepository.Verify(x => x.InsertAsync(It.IsAny<ActivityAnswer>()), Times.Never);
        _mockActivityAnswerVersionRepository.Verify(x => x.InsertAsync(It.IsAny<ActivityAnswerVersion>()), Times.Never);
        _mockActivityAnswerRepository.Verify(x => x.SetLastReview(It.IsAny<string>(), It.IsAny<ActivityAnswerVersion>()), Times.Never);
    }
}
