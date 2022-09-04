using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Controllers.Base;
using SchoolApp.IdentityProvider.Api.Mappers;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.Api.Controllers;

public class TeachersController : BaseController
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_teacherService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] TeacherCreateModel payload)
    {
        return Ok(await _teacherService.CreateAsync(GetAuthenticatedUser(), payload.MapToTeacher()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] TeacherUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _teacherService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToTeacher()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _teacherService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
