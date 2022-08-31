using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [Route("TeacherLogin")]
    [AllowAnonymous]
    public IActionResult TeacherLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Teacher));
    }

    [Route("ManagerLogin")]
    [AllowAnonymous]
    public IActionResult ManagerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Manager));
    }

    [Route("OwnerLogin")]
    [AllowAnonymous]
    public IActionResult OwnerLogin([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password, Application.Domain.Enums.UserTypeEnum.Owner));
    }
}
