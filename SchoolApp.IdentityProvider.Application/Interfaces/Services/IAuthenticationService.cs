using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IAuthenticationService
{
    AuthenticationObject Login(string email, string password, UserTypeEnum userType);
}
