using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : Controller
{
    private readonly ITeacherService _teacherService;
    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet("GetOneById/{id}")]
    public IActionResult GetOneById(int id)
    {
        return Ok(_teacherService.GetOneById(id));
    }
}
