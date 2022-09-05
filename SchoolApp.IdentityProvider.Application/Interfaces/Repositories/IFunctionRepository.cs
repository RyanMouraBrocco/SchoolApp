using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IFunctionRepository : ICrudRepository<Function>
{
    IList<Function> GetAll(int accountId, int top, int skip);
}
