using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Api.Mappers;
using SchoolApp.Classroom.Api.Models.Subjects;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.Api.Controllers;

public class SubjectController : BaseController
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService studentService)
    {
        _subjectService = studentService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_subjectService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] SubjectModel payload)
    {
        return Ok(await _subjectService.CreateAsync(GetAuthenticatedUser(), payload.MapToSubject()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] SubjectModel payload, [FromRoute] int id)
    {
        return Ok(await _subjectService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToSubject()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _subjectService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
