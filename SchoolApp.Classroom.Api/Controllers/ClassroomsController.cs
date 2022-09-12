using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Api.Mappers;
using SchoolApp.Classroom.Api.Models.Classrooms;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.Api.Controllers;

public class ClassroomsController : BaseController
{
    private readonly IClassroomService _classroomServie;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomServie = classroomService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_classroomServie.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ClassroomCreateModel payload)
    {
        return Ok(await _classroomServie.CreateAsync(GetAuthenticatedUser(), payload.MapToClassroom()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] ClassroomUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _classroomServie.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToClassroom()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _classroomServie.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }

}
