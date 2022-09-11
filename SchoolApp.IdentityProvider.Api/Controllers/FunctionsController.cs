using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.IdentityProvider.Api.Models;
using SchoolApp.IdentityProvider.Api.Models.Functions;
using SchoolApp.IdentityProvider.Application.Interfaces.Services;
using SchoolApp.IdentityProvider.Api.Mappers;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.IdentityProvider.Api.Controllers;

public class FunctionsController : BaseController
{
    private readonly IFunctionService _functionService;
    public FunctionsController(IFunctionService functionService)
    {
        _functionService = functionService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_functionService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] FunctionModel payload)
    {
        return Ok(await _functionService.CreateAsync(GetAuthenticatedUser(), payload.MapToFunction()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] FunctionModel payload, [FromRoute] int id)
    {
        return Ok(await _functionService.UpdateAsync(GetAuthenticatedUser(), id, payload.MapToFunction()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _functionService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }
}
