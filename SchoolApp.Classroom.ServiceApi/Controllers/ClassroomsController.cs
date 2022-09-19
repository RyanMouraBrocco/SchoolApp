using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Models;

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

    [HttpGet("GetAllByOwnerId/{ownerId}")]
    public IActionResult GetAllByOwnerId(int ownerId, [FromQuery] PagingModel paging)
    {
        return Ok(_classroomService.GetAllByOwnerId(ownerId, paging.Top, paging.Skip));
    }

    [HttpGet("GetAllByTeacherId/{teacherId}")]
    public IActionResult GetAllByTeacherId(int teacherId, [FromQuery] PagingModel paging)
    {
        return Ok(_classroomService.GetAllByTeacherId(teacherId, paging.Top, paging.Skip));
    }
}
