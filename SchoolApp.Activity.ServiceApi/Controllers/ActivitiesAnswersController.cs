using System.Net;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Activity.Application.Interfaces.Services;

namespace SchoolApp.Activity.ServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesAnswersController : Controller
{
    private readonly IActivityAnswerService _activityAnswerService;

    public ActivitiesAnswersController(IActivityAnswerService activityAnswerService)
    {
        _activityAnswerService = activityAnswerService;
    }

    [HttpGet("GetOneByIdIncludingActivity/{activityAnswerId}")]
    public IActionResult GetOneByIdIncludingActivity(string activityAnswerId)
    {
        return Ok(_activityAnswerService.GetOneByIdIncludingActivity(activityAnswerId));
    }
}
