using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.File.Api.Mappers;
using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;

namespace SchoolApp.File.Api.Controllers;

public class UsersFilesController : BaseController
{
    private readonly IUserFileService _userFileService;
    public UsersFilesController(IUserFileService userFileService)
    {
        _userFileService = userFileService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get()
    {
        return Ok(_userFileService.GetAll(GetAuthenticatedUser()));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] UserFileCreateModel payload)
    {
        await _userFileService.AddAsync(GetAuthenticatedUser(), payload.MapToUserFile());
        return Ok();
    }

    [HttpDelete()]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromBody] UserFileRemoveModel payload)
    {
        await _userFileService.RemoveAsync(GetAuthenticatedUser(), payload.MapToUserFile());
        return Ok();
    }
}
