using SchoolApp.IdentityProvider.Application.Domain.Users;

namespace SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

public class Owner : User
{
    public bool IsMainOwner { get; set; }
}
