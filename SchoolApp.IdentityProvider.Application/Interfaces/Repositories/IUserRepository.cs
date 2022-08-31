using SchoolApp.IdentityProvider.Application.Domain.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IUserRepository<TUser> where TUser : User
{
    TUser GetOneByEmail(string email);
}
