using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IUserRepository<TUser> : ICrudRepository<TUser, int> where TUser : User
{
    TUser GetOneByEmail(string email);
    IList<TUser> GetAll(int accountId, int top, int skip);
}
