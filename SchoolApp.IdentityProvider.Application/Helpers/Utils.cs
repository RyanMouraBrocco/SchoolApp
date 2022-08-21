using System.Security.Cryptography;
using System.Text;

namespace SchoolApp.IdentityProvider.Application.Helpers;

public static class Utils
{
    public static string HashText(string unhashedText)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] unhashedTextBytes = Encoding.UTF8.GetBytes(unhashedText);
            byte[] hashedTextBytes = sha256Hash.ComputeHash(unhashedTextBytes);
            return BitConverter.ToString(hashedTextBytes).Replace("-", String.Empty);
        }
    }
}
