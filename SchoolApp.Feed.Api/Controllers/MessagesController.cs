using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Feed.Api.Mappers;
using SchoolApp.Feed.Api.Models;
using SchoolApp.Feed.Application.Domain.Dtos;
using SchoolApp.Feed.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;
using SchoolApp.Shared.Utils.HttpApi.Models;

namespace SchoolApp.Feed.Api.Controllers;

public class MessagesController : BaseController
{
    private readonly IMessageService _messageService;
    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    [Authorize()]
    public async Task<IActionResult> GetAsync([FromQuery] PagingModel paging)
    {
        return Ok(await _messageService.GetAllMainMessagesAsync(GetAuthenticatedUser(), paging.Top, paging.Skip));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] MessageCreateModel payload)
    {
        return Ok(await _messageService.CreateAsync(GetAuthenticatedUser(),
                                                    payload.MapToMessage(),
                                                    payload.AllowedClassrooms.Select(x => new MessageAllowedClassroomDto() { ClassroomId = x.ClassroomId }).ToList(),
                                                    payload.AllowedStudents.Select(x => new MessageAllowedStudentDto() { StudentId = x.StudentId }).ToList()));
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> PutAsync([FromBody] MessageUpdateModel payload, [FromRoute] string id)
    {
        return Ok(await _messageService.UpdateAsync(GetAuthenticatedUser(),
                                                    id,
                                                    payload.MapToMessage(),
                                                    payload.AllowedClassrooms.Select(x => new MessageAllowedClassroomDto() { ClassroomId = x.ClassroomId }).ToList(),
                                                    payload.AllowedStudents.Select(x => new MessageAllowedStudentDto() { StudentId = x.StudentId }).ToList()));
    }

    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        await _messageService.DeleteAsync(GetAuthenticatedUser(), id);
        return Ok();
    }

}
