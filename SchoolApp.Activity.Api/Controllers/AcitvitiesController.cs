using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Activity.Api.Mappers;
using SchoolApp.Activity.Api.Models.Activities;
using SchoolApp.Activity.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Activity.Api.Controllers;

public class AcitvitiesController : BaseController
{
    private readonly IActivityService _activityService;
    public AcitvitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_activityService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ActivityCreateModel payload)
    {
        return Ok(await _activityService.CreateAsync(GetAuthenticatedUser(), payload.MapToActivity()));
    }

    [HttpPost("{id}/Close")]
    [Authorize()]
    public async Task<IActionResult> CloseAsync(string id)
    {
        return Ok(await _activityService.CloseAsync(GetAuthenticatedUser(), id));
    }

    [HttpPost("{id}/Open")]
    [Authorize()]
    public async Task<IActionResult> OpenAsync(string id)
    {
        return Ok(await _activityService.OpenAsync(GetAuthenticatedUser(), id));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] ActivityUpdateModel payload, [FromRoute] string id)
    {
        return Ok(await _activityService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToActivity()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        await _activityService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
