using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Application.Interfaces.Services;

namespace SchoolApp.Classroom.ServiceApi.Controllers;

public class ClassroomsController : Controller
{
    private readonly IClassroomService _classroomService;
    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet("GetOneById/{id}")]
    public IActionResult GetOneById(int id)
    {
        return Ok(_classroomService.GetOneById(id));
    }
}
