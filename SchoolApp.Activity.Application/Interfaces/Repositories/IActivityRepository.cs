using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IActivityRepository : ICrudRepository<Domain.Entities.Activities.Activity, string>
{
    IList<Domain.Entities.Activities.Activity> GetAll(int accountId, int top, int skip);
    IList<Application.Domain.Entities.Activities.Activity> GetAllByClassroomsIds(IEnumerable<int> classroomsIds);
}
