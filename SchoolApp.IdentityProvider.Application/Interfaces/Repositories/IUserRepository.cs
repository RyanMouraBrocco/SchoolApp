using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Repositories;

public interface IUserRepository<TUser> where TUser : User
{
    Task<TUser> InsertAsync(TUser item);
    Task<TUser> UpdateAsync(TUser item);
    Task DeleteAsync(int id);
    TUser GetOneById(int id);
    TUser GetOneByEmail(string email);
    IList<TUser> GetAll(int accountId, int top, int skip);
}
