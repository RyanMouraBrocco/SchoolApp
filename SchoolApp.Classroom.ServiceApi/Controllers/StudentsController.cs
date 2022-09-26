using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : Controller
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("GetAllByOwnerId/{ownerId}")]
    public IActionResult GetAllByOwnerId(int ownerId, [FromQuery] PagingModel paging)
    {
        return Ok(_studentService.GetAllByOwnerId(ownerId, paging.Top, paging.Skip));
    }

}
