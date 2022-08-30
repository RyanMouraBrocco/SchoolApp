using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.Api.Controllers;

[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    public IActionResult Post([FromBody] AuthenticationLoginModel loginModel)
    {
        return Ok(_authenticationService.Login(loginModel.Login, loginModel.Password));
    }
}
