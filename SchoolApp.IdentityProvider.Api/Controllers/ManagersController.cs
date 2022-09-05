using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Controllers.Base;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Api.Mappers;

namespace SchoolApp.IdentityProvider.Api.Controllers;

public class ManagersController : BaseController
{
    private readonly IManagerService _managerService;
    public ManagersController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_managerService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ManagerCreateModel payload)
    {
        return Ok(await _managerService.CreateAsync(GetAuthenticatedUser(), payload.MapToManager()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] ManagerUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _managerService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToManager()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _managerService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
