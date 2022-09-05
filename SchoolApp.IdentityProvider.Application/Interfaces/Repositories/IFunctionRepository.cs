using SchoolApp.IdentityProvider.Application.Domain.Entities.Functions;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IFunctionRepository
{
    Function GetOneById(int id);
}
