using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.Enums;
using SchoolApp.Shared.Utils.HttpApi.Controllers;

namespace SchoolApp.IdentityProvider.Api.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("TeacherLogin")]
    [AllowAnonymous]
    public IActionResult TeacherLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, UserTypeEnum.Teacher));
    }

    [HttpPost("ManagerLogin")]
    [AllowAnonymous]
    public IActionResult ManagerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, UserTypeEnum.Manager));
    }

    [HttpPost("OwnerLogin")]
    [AllowAnonymous]
    public IActionResult OwnerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, UserTypeEnum.Owner));
    }
}
