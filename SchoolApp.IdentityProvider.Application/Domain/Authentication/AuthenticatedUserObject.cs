using SchoolApp.IdentityProvider.Application.Domain.Enums;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;

namespace SchoolApp.IdentityProvider.Application.Domain.Authentication;

public class AuthenticatedUserObject
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int AccountId { get; set; }
    public UserTypeEnum Type { get; set; }
}
