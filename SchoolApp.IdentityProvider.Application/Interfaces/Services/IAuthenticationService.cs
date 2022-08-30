using SchoolApp.IdentityProvider.Application.Domain.Authentication;

namespace SchoolApp.IdentityProvider.Application.Interfaces.Services;

public interface IAuthenticationService
{
    AuthenticationObject Login(string email, string password);
}
