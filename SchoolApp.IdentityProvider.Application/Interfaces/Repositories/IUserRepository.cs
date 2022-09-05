using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IUserRepository<TUser> : ICrudRepository<TUser> where TUser : User
{
    TUser GetOneByEmail(string email);
    IList<TUser> GetAll(int accountId, int top, int skip);
}
