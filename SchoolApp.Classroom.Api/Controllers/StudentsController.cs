using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Api.Mappers;
using SchoolApp.Classroom.Api.Models.Students;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.Api.Controllers;

public class StudentsController : BaseController
{
    private readonly IStudentService _studentServie;

    public StudentsController(IStudentService studentService)
    {
        _studentServie = studentService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_studentServie.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] StudentCreateModel payload)
    {
        return Ok(await _studentServie.CreateAsync(GetAuthenticatedUser(), payload.MapToStudent()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] StudentUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _studentServie.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToStudent()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _studentServie.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
