using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.IdentityProvider.Test.Utils;

public static class Helper
{
    public static AuthenticatedUserObject CreateRequesterUser1(UserTypeEnum userType)
    {
        return new AuthenticatedUserObject()
        {
            UserId = 1,
            AccountId = 1,
            Type = userType,
            UserEmail = "e@email.com",
            UserName = "John"
        };
    }
}
