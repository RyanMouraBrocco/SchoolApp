using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IUserService<TUser> : ICrudService<TUser> where TUser : User
{
}
