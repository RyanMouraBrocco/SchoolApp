using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnersController : Controller
{
    private readonly IOwnerService _onwerService;
    public OwnersController(IOwnerService ownerService)
    {
        _onwerService = ownerService;
    }

    [HttpGet("GetOneById/{id}")]
    public IActionResult GetOneById(int id)
    {
        return Ok(_onwerService.GetOneById(id));
    }
}
