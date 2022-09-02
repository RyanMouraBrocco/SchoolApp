using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IUserService<TUser> where TUser : User
{
    IList<TUser> GetAll(AuthenticatedUserObject requesterUser, int top, int skip);
    Task<TUser> CreateAsync(AuthenticatedUserObject requesterUser, TUser newUser);
    Task<TUser> UpdateAsync(AuthenticatedUserObject requesterUser, int userId, TUser updatedUser);
    Task DeleteAsync(AuthenticatedUserObject requesterUser, int userId);
}
