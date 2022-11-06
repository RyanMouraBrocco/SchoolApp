using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Shared.Utils.Test.Helpers;

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

    public static object CreateRequesterUser1(object owner)
    {
        throw new NotImplementedException();
    }
}
