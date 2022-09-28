using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.File.Api.Mappers;
using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;

namespace SchoolApp.File.Api.Controllers;

public class ActivitiesAnswersVersionsFilesController : BaseController
{
    private readonly IActivityAnswerVersionFileService _activityAnswerVersionFileService;
    public ActivitiesAnswersVersionsFilesController(IActivityAnswerVersionFileService activityAnswerVersionFileService)
    {
        _activityAnswerVersionFileService = activityAnswerVersionFileService;
    }

    [HttpGet("GetAllByActivityAnswerVersionId/{activityAnswerVersionId}")]
    [Authorize()]
    public async Task<IActionResult> GetAllByActivityIdAsync([FromQuery] string activityAnswerVersionId)
    {
        return Ok(await _activityAnswerVersionFileService.GetAllByActivityAnswerVersionIdAsync(GetAuthenticatedUser(), activityAnswerVersionId));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ActivityAnswerVersionFileCreateModel payload)
    {
        await _activityAnswerVersionFileService.AddAsync(GetAuthenticatedUser(), payload.MapToActivityAnswerVersionFile());
        return Ok();
    }

    [HttpDelete()]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromBody] ActivityAnswerVersionFileRemoveModel payload)
    {
        await _activityAnswerVersionFileService.RemoveAsync(GetAuthenticatedUser(), payload.MapToActivityAnswerVersionFile());
        return Ok();
    }
}
