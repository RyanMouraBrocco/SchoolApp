using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Activity.Application.Interfaces.Repositories;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.Validations;

namespace SchoolApp.Activity.Application.Services;

public class ActivityAnswerService : IActivityAnswerService
{
    private readonly IActivityAnswerRepository _activityAnswerRepository;
    private readonly IActivityAnswerVersionRepository _activityAnswerVersionRepository;
    private readonly IActivityService _activityService;
    private readonly IActivityRepository _activityRepository;
    private readonly IStudentRepository _studentRepository;

    public ActivityAnswerService(IActivityAnswerRepository activityAnswerRepository,
                                 IActivityAnswerVersionRepository activityAnswerVersionRepository,
                                 IActivityService activityService,
                                 IActivityRepository activityRepository,
                                 IStudentRepository studentRepository)
    {
        _activityAnswerRepository = activityAnswerRepository;
        _activityAnswerVersionRepository = activityAnswerVersionRepository;
        _activityService = activityService;
        _activityRepository = activityRepository;
        _studentRepository = studentRepository;
    }

    public async Task<ActivityAnswer> CreateAsync(AuthenticatedUserObject requesterUser, ActivityAnswer newActivityAnswer)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);

        if (newActivityAnswer.LastReview == null || string.IsNullOrEmpty(newActivityAnswer.LastReview.Text?.Trim()))
            throw new FormatException("LastReview text can't not be null or empty");

        var activityCheck = await _activityService.GetOneByIdAsync(requesterUser, newActivityAnswer.ActivityId);
        if (activityCheck == null)
            throw new UnauthorizedAccessException("Acitivity not found");

        if (activityCheck.Closed)
            throw new UnauthorizedAccessException("Activity is closed");

        var allStudents = await _studentRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
        var studentCheck = allStudents.FirstOrDefault(x => x.Id == newActivityAnswer.StudentId);
        if (studentCheck == null)
            throw new UnauthorizedAccessException("Student not found");

        newActivityAnswer.AccountId = requesterUser.AccountId;
        newActivityAnswer.CreationDate = DateTime.Now;

        var lastReview = newActivityAnswer.LastReview;

        var insertedActitivityAnswer = await _activityAnswerRepository.InsertAsync(newActivityAnswer);
        lastReview.ActivityAnswerId = insertedActitivityAnswer.Id;
        var insertedLastReview = await _activityAnswerVersionRepository.InsertAsync(lastReview);
        await _activityAnswerRepository.SetLastReview(newActivityAnswer.Id, insertedLastReview);

        return newActivityAnswer;
    }

    public async Task<ActivityAnswer> CreateReviewAsync(AuthenticatedUserObject requesterUser, string activityAnswerId, ActivityAnswerVersion newReview)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);

        if (string.IsNullOrEmpty(newReview.Text?.Trim()))
            throw new FormatException("Text can't not be null or empty");

        var activityAnswerCheck = _activityAnswerRepository.GetOneById(activityAnswerId);
        if (activityAnswerCheck == null || activityAnswerCheck.AccountId != requesterUser.AccountId)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        var allStudents = await _studentRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
        if (!allStudents.Any(x => x.Id == activityAnswerCheck.StudentId))
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        var activityCheck = await _activityService.GetOneByIdAsync(requesterUser, activityAnswerCheck.ActivityId);
        if (activityCheck.Closed)
            throw new UnauthorizedAccessException("Activity is closed");

        newReview.ActivityAnswerId = activityAnswerId;
        newReview.CreationDate = DateTime.Now;

        var insertedReview = await _activityAnswerVersionRepository.InsertAsync(newReview);
        await _activityAnswerRepository.SetLastReview(activityAnswerId, insertedReview);

        return _activityAnswerRepository.GetOneById(activityAnswerId);
    }

    public async Task<IList<ActivityAnswer>> GetAllByActivityIdAsync(AuthenticatedUserObject requesterUser, string activityId, int top, int skip)
    {
        var activityCheck = await _activityService.GetOneByIdAsync(requesterUser, activityId);
        if (activityCheck == null)
            return new List<ActivityAnswer>();

        if (requesterUser.Type == UserTypeEnum.Manager || requesterUser.Type == UserTypeEnum.Teacher)
            return _activityAnswerRepository.GetAllByActitvityId(activityId, top, skip);
        else if (requesterUser.Type == UserTypeEnum.Owner)
        {
            var students = await _studentRepository.GetAllByOwnerIdAsync(requesterUser.UserId);
            return _activityAnswerRepository.GetAllByActitvityIdAndStudentsIds(activityId, students.Select(x => x.Id));
        }
        else
            throw new NotImplementedException("User not valid");
    }

    public ActivityAnswer GetOneByIdIncludingActivity(string activityAnswerId)
    {
        var activityAnswer = _activityAnswerRepository.GetOneById(activityAnswerId);
        if (activityAnswer != null)
        {
            activityAnswer.Activity = _activityRepository.GetOneById(activityAnswer.ActivityId);
        }

        return activityAnswer;
    }
}
