using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Interfaces.Services;

public interface IActivityFileService : IFileService<ActivityFile>
{
    Task<IList<ActivityFile>> GetAllByActivityIdAsync(AuthenticatedUserObject requesterUser, string activityId);
}
