using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Shared.Authentication;
using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Shared.Utils.HttpApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    public BaseController()
    {

    }

    protected AuthenticatedUserObject GetAuthenticatedUser()
    {
        var jwtUser = HttpContext.User;

        return new AuthenticatedUserObject()
        {
            UserId = int.Parse(jwtUser.Claims.FirstOrDefault(x => x.Type == "Id").Value),
            UserName = jwtUser.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value,
            AccountId = int.Parse(jwtUser.Claims.FirstOrDefault(x => x.Type == "AccountId").Value),
            Type = (UserTypeEnum)int.Parse(jwtUser.Claims.FirstOrDefault(x => x.Type == "Type").Value)
        };
    }
}

