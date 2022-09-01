using System.Text.RegularExpressions;
using SchoolApp.IdentityProvider.Application.Domain.Enums;

namespace SchoolApp.IdentityProvider.Application.Validations;

public static class UserValidation
{
    public static bool IsSecurityPassword(string password)
    {
        if (password.Length < 8)
            return false;

        var upperCaseRegex = new Regex("[A-Z]+");
        var lowerCaseRegex = new Regex("[a-z]+");
        var numberRegex = new Regex("[0-9]+");

        return upperCaseRegex.IsMatch(password) &&
               lowerCaseRegex.IsMatch(password) &&
               numberRegex.IsMatch(password);
    }

    public static void CheckOnlyManagerUser(UserTypeEnum userType)
    {
        if (userType != UserTypeEnum.Manager)
            throw new UnauthorizedAccessException("This resource is just to manager users");
    }
}
