using System.Text.RegularExpressions;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.Shared.Utils.Enums;

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

    public static void CheckUserFields(User user)
    {
        if (string.IsNullOrEmpty(user.Name?.Trim()))
        {
            throw new FormatException("Name can't be null or empty");
        }

        if (string.IsNullOrEmpty(user.Email?.Trim()))
        {
            throw new FormatException("Email can't be null or empty");
        }

        if (string.IsNullOrEmpty(user.Password?.Trim()))
        {
            throw new FormatException("Password can't be null or empty");
        }

        if (string.IsNullOrEmpty(user.DocumentId?.Trim()))
        {
            throw new FormatException("DocumentId can't be null or empty");
        }
    }
}
