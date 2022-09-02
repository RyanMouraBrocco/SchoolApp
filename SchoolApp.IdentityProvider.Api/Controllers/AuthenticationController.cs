using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Controllers.Base;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

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
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Teacher));
    }

    [HttpPost("ManagerLogin")]
    [AllowAnonymous]
    public IActionResult ManagerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Manager));
    }

    [HttpPost("OwnerLogin")]
    [AllowAnonymous]
    public IActionResult OwnerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Owner));
    }
}
