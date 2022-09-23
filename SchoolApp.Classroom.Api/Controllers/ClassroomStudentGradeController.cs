using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Api.Mappers;
using SchoolApp.Classroom.Api.Models.Grades;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.Api.Controllers;

public class ClassroomStudentGradeController : BaseController
{
    private readonly IClassroomStudentGradeService _classroomStudentGradeService;

    public ClassroomStudentGradeController(IClassroomStudentGradeService classroomStudentGradeService)
    {
        _classroomStudentGradeService = classroomStudentGradeService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_classroomStudentGradeService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ClassroomStudentGradeCreateModel payload)
    {
        return Ok(await _classroomStudentGradeService.CreateAsync(GetAuthenticatedUser(), payload.MapToClassroomStudentGrade()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] ClassroomStudentGradeUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _classroomStudentGradeService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToClassroomStudentGrade()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _classroomStudentGradeService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
