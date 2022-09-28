using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.File.Api.Mappers;
using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;

namespace SchoolApp.File.Api.Controllers;

public class ActivitiesFilesController : BaseController
{
    private readonly IActivityFileService _activityFileService;
    public ActivitiesFilesController(IActivityFileService activityFileService)
    {
        _activityFileService = activityFileService;
    }

    [HttpGet("GetAllByActivityId/{activityId}")]
    [Authorize()]
    public async Task<IActionResult> GetAllByActivityIdAsync([FromQuery] string activityId)
    {
        return Ok(await _activityFileService.GetAllByActivityIdAsync(GetAuthenticatedUser(), activityId));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ActivityFileCreateModel payload)
    {
        await _activityFileService.AddAsync(GetAuthenticatedUser(), payload.MapToActivityFile());
        return Ok();
    }

    [HttpDelete()]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromBody] ActivityFileRemoveModel payload)
    {
        await _activityFileService.RemoveAsync(GetAuthenticatedUser(), payload.MapToActivityFile());
        return Ok();
    }
}
