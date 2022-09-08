using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Shared.Authentication;

public class AuthenticatedUserObject
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int AccountId { get; set; }
    public UserTypeEnum Type { get; set; }
}