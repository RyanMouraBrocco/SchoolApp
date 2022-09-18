using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Services;

public interface IActivityAnswerService
{
    Task<ActivityAnswer> CreateAsync(AuthenticatedUserObject requesterUser, ActivityAnswer newActivityAnswer);
    IList<ActivityAnswer> GetAllByActivityId(AuthenticatedUserObject requesterUser, string activityId, int top, int skip);
    Task<ActivityAnswer> CreateReviewAsync(AuthenticatedUserObject requesterUser, string itemId, ActivityAnswerVersion newReview);
}
