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

    public ActivityAnswerService(IActivityAnswerRepository activityAnswerRepository,
                                 IActivityAnswerVersionRepository activityAnswerVersionRepository,
                                 IActivityService activityService)
    {
        _activityAnswerRepository = activityAnswerRepository;
        _activityAnswerVersionRepository = activityAnswerVersionRepository;
        _activityService = activityService;
    }

    public async Task<ActivityAnswer> CreateAsync(AuthenticatedUserObject requesterUser, ActivityAnswer newActivityAnswer)
    {
        GenericValidation.CheckOnlyOwnerUser(requesterUser.Type);

        if (newActivityAnswer.LastReview == null || string.IsNullOrEmpty(newActivityAnswer.LastReview.Text?.Trim()))
            throw new FormatException("LastReview text can't not be null or empty");

        var activityCheck = await _activityService.GetOneByIdAsync(requesterUser, newActivityAnswer.ActivityId);
        if (activityCheck == null)
            throw new UnauthorizedAccessException("Acitivity not found");

        newActivityAnswer.AccountId = requesterUser.AccountId;
        newActivityAnswer.StudentId = requesterUser.UserId;
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

        var activityAnswerCheck = _activityAnswerRepository.GetOneById(activityAnswerId);
        if (activityAnswerCheck == null || activityAnswerCheck.AccountId != requesterUser.AccountId || activityAnswerCheck.StudentId != requesterUser.UserId)
            throw new UnauthorizedAccessException("ActivityAnswer not found");

        if (string.IsNullOrEmpty(newReview.Text?.Trim()))
            throw new FormatException("Text can't not be null or empty");

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
            // need to search all students of owner first
            return new List<ActivityAnswer>();
        }
        else
            throw new NotImplementedException("User not valid");
    }
}
