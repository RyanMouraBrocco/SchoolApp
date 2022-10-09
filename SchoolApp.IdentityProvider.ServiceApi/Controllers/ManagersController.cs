using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagersController : Controller
{
    private readonly IManagerService _managerService;
    public ManagersController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    [HttpGet("GetOneById/{id}")]
    public IActionResult GetOneById(int id)
    {
        return Ok(_managerService.GetOneById(id));
    }
}
