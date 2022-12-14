using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Activity.Api.Mappers;
using SchoolApp.Activity.Api.Models.ActivitiesAnswers;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Activity.Api.Controllers;

public class ActivitiesAnswersController : BaseController
{
    private readonly IActivityAnswerService _activityAnswerService;
    public ActivitiesAnswersController(IActivityAnswerService activityAnswerService)
    {
        _activityAnswerService = activityAnswerService;
    }

    [HttpGet("GetAllByActivityId/{activityId}")]
    [Authorize()]
    public async Task<IActionResult> GetAllByActivityIdAsync(string activityId, [FromQuery] PagingModel paging)
    {
        return Ok(await _activityAnswerService.GetAllByActivityIdAsync(GetAuthenticatedUser(), activityId, paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ActivityAnswerCreateModel payload)
    {
        return Ok(await _activityAnswerService.CreateAsync(GetAuthenticatedUser(), payload.MapToActivityAnswer()));
    }

    [HttpPost("Review/{id}")]
    [Authorize()]
    public async Task<IActionResult> ReviewAsync([FromBody] ActivityAnswerCreateReviewModel payload, [FromRoute] string id)
    {
        return Ok(await _activityAnswerService.CreateReviewAsync(GetAuthenticatedUser(), id, payload.MapToActivityAnswerVersion()));
    }
}
