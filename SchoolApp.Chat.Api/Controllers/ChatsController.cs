using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Chat.Api.Mappers;
using SchoolApp.Chat.Api.Models;
using SchoolApp.Chat.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Chat.Api.Controllers;

public class ChatsController : BaseController
{
    private readonly IChatService _chatService;

    public ChatsController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    [Authorize()]
    public IActionResult Get([FromQuery] PagingModel paging)
    {
        return Ok(_chatService.GetAll(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] ChatCreateModel payload)
    {
        return Ok(await _chatService.CreateAsync(GetAuthenticatedUser(), payload.MapToChat()));
    }
}
