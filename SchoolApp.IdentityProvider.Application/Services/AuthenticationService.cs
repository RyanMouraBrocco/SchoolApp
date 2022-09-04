using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolApp.IdentityProvider.Application.Domain.Authentication;
using SchoolApp.IdentityProvider.Application.Domain.Enums;
using SchoolApp.IdentityProvider.Application.Domain.Entities.Users;
using SchoolApp.IdentityProvider.Application.Helpers;
using SchoolApp.IdentityProvider.Application.Interfaces.Repositories;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Application.Settings;

namespace SchoolApp.IdentityProvider.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private AuthenticationSettings AuthenticationSettings { get; set; }
    private readonly ITeacherRepository _teacherRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IManagerRepository _managerRepository;
    public AuthenticationService(ITeacherRepository teacherRepository,
                                 IOwnerRepository ownerRepository,
                                 IManagerRepository managerRepository,
                                 IOptions<AuthenticationSettings> settings)
    {
        _teacherRepository = teacherRepository;
        _ownerRepository = ownerRepository;
        _managerRepository = managerRepository;
        AuthenticationSettings = settings.Value;
    }

    public AuthenticationObject Login(string email, string password, UserTypeEnum userType)
    {
        var fetchedUser = GetUserByEmailDependingOnUserType(email, userType);
        if (fetchedUser == null)
            throw new UnauthorizedAccessException("Email or password was wrong");

        var hashedPassword = Utils.HashText(password);
        if (!fetchedUser.Password.Equals(hashedPassword))
            throw new UnauthorizedAccessException("Email or password was wrong");


        fetchedUser.Password = null;
        return new AuthenticationObject()
        {
            Key = GenerateJsonToken(fetchedUser, userType),
            User = fetchedUser
        };
    }

    private User GetUserByEmailDependingOnUserType(string email, UserTypeEnum userType) => userType switch
    {
        UserTypeEnum.Teacher => _teacherRepository.GetOneByEmail(email),
        UserTypeEnum.Manager => _managerRepository.GetOneByEmail(email),
        UserTypeEnum.Owner => _ownerRepository.GetOneByEmail(email),
        _ => throw new NotImplementedException("This user type is not valid")
    };

    private string GenerateJsonToken(User user, UserTypeEnum userType)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim("Type", ((int)userType).ToString()),
                new Claim("AccountId", user.AccountId.ToString())
            };
        var token = new JwtSecurityToken(AuthenticationSettings.Issuer, AuthenticationSettings.Issuer, claims, expires: DateTime.Now.AddMinutes(AuthenticationSettings.ExpirationTimeInMinutes), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
