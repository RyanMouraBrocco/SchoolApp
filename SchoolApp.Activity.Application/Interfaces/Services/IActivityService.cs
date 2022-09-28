using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Services;

public interface IActivityService : ICrudService<Domain.Entities.Activities.Activity, string>
{
    Domain.Entities.Activities.Activity GetOneById(string id);
    Task<Domain.Entities.Activities.Activity> GetOneByIdAsync(AuthenticatedUserObject requesterUser, string id);
    Task<Domain.Entities.Activities.Activity> CloseAsync(AuthenticatedUserObject requesterUser, string activityId);
    Task<Domain.Entities.Activities.Activity> OpenAsync(AuthenticatedUserObject requesterUser, string activityId);
}
