namespace SchoolApp.IdentityProvider.Application.Settings;

public class AuthenticationSettings
{
    public string Issuer { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
    public string Key { get; set; }
}
