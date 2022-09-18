using SchoolApp.Activity.Application.Domain.Entities.Answers;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IActivityAnswerRepository : ICrudRepository<ActivityAnswer, string>
{
    Task SetLastReview(string id, ActivityAnswerVersion version);
}
