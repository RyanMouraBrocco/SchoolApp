using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Services;

public interface IActivityService : ICrudService<Domain.Entities.Activities.Activity, string>
{
    Task<Domain.Entities.Activities.Activity> GetOneByIdAsync(AuthenticatedUserObject requesterUser, string id);
}
