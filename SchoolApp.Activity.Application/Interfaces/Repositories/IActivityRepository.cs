using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.Activity.Application.Interfaces.Repositories;

public interface IActivityRepository : ICrudRepository<Domain.Entities.Activities.Activity, string>
{
}
