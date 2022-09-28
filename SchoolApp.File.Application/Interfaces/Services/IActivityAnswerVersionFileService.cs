using SchoolApp.File.Application.Domain.Entities;
using SchoolApp.Shared.Authentication;

namespace SchoolApp.File.Application.Interfaces.Services;

public interface IActivityAnswerVersionFileService : IFileService<ActivityAnswerVersionFile>
{
    Task<IList<ActivityAnswerVersionFile>> GetAllByActivityAnswerVersionIdAsync(AuthenticatedUserObject requesterUser, string activityAnswerVersionId);
}
