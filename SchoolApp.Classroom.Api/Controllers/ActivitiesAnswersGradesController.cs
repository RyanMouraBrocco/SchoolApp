using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Classroom.Api.Mappers;
using SchoolApp.Classroom.Api.Models.Grades;
using SchoolApp.Classroom.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Classroom.Api.Controllers;

public class ActivitiesAnswersGradesController : BaseController
{
    private readonly IActivityAnswerGradeService _activityAnswerGradeService;

    public ActivitiesAnswersGradesController(IActivityAnswerGradeService activityAnswerGradeService)
    {
        _activityAnswerGradeService = activityAnswerGradeService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_activityAnswerGradeService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ActivityAnswerGradeCreateModel payload)
    {
        return Ok(await _activityAnswerGradeService.CreateAsync(GetAuthenticatedUser(), payload.MapToActivityAnswerGrade()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] ActivityAnswerGradeUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _activityAnswerGradeService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToActivityAnswerGrade()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _activityAnswerGradeService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
