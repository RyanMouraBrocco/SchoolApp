using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Users;
using SchoolApp.IdentityProvider.Application.Helpers;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Settings;

namespace SchoolApp.IdentityProvider.Application.Services;

public class AuthenticationService
{
    private AuthenticationSettings AuthenticationSettings { get; set; }
    private readonly IUserRepository _userRepository;
    public AuthenticationService(IUserRepository userRepository, AuthenticationSettings settings)
    {
        _userRepository = userRepository;
        AuthenticationSettings = settings;
    }

    public AuthenticationObject Login(string email, string password)
    {
        var fetchedUser = _userRepository.GetOneByEmail(email);
        if (fetchedUser == null)
            throw new UnauthorizedAccessException("Email or password was wrong");

        var hashedPassword = Utils.HashText(password);
        if (!fetchedUser.Password.Equals(hashedPassword))
            throw new UnauthorizedAccessException("Email or password was wrong");


        fetchedUser.Password = null;
        return new AuthenticationObject()
        {
            Key = GenerateJsonToken(fetchedUser),
            User = fetchedUser
        };
    }

    private string GenerateJsonToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
        var token = new JwtSecurityToken(AuthenticationSettings.Issuer, AuthenticationSettings.Issuer, claims, expires: DateTime.Now.AddMinutes(AuthenticationSettings.ExpirationTimeInMinutes), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
