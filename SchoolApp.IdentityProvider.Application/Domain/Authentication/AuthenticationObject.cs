using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Domain.Authentication;

public class AuthenticationObject
{
    public User User { get; set; }
    public string Key { get; set; }
}
