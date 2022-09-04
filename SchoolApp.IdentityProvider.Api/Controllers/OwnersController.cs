using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Controllers.Base;
using SchoolApp.IdentityProvider.Api.Mappers;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Api.Models.Users;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;

namespace SchoolApp.IdentityProvider.Api.Controllers;

public class OwnersController : BaseController
{
    private readonly IOwnerService _ownerService;
    public OwnersController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_ownerService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] OwnerCreateModel payload)
    {
        return Ok(await _ownerService.CreateAsync(GetAuthenticatedUser(), payload.MapToOwner()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] OwnerUpdateModel payload, [FromRoute] int id)
    {
        return Ok(await _ownerService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToOwner()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _ownerService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
