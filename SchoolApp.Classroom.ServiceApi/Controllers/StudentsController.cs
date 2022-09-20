using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.ServiceApi.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("GetAllByOwnerId/{ownerId}")]
    public IActionResult GetOneById(int ownerId, [FromQuery] PagingModel paging)
    {
        return Ok(_studentService.GetAllByOwnerId(ownerId, paging.Top, paging.Skip));
    }

}
