using Microsoft.AspNetCore.Mvc;
using SchoolApp.Activity.Application.Interfaces.Services;

namespace SchoolApp.Activity.ServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : Controller
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet("GetOneById/{activityId}")]
    public IActionResult GetOneById(string activityId)
    {
        return Ok(_activityService.GetOneById(activityId));
    }
}
