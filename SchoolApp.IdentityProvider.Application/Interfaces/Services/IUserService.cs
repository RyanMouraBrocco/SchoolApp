using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.Shared.Utils.Interfaces;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IUserService<TUser> : ICrudService<TUser> where TUser : User
{
}
