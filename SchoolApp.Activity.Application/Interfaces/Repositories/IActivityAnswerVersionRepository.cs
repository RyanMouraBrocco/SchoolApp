using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IActivityAnswerVersionRepository : ICrudRepository<ActivityAnswerVersion, string>
{
}
