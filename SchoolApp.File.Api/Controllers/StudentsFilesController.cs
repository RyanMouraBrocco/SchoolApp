using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.File.Api.Mappers;
using SchoolApp.File.Api.Models;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.Shared.Utils.HttpApi.Controllers;

namespace SchoolApp.File.Api.Controllers;

public class StudentsFilesController : BaseController
{
    private readonly IStudentFileService _studentFileService;
    public StudentsFilesController(IStudentFileService studentFileService)
    {
        _studentFileService = studentFileService;
    }

    [HttpGet("GetAllByStudentId/{studentId}")]
    [Authorize()]
    public async Task<IActionResult> GetAllByStudentIdAsync([FromQuery] int studentId)
    {
        return Ok(await _studentFileService.GetAllByStudentIdAsync(GetAuthenticatedUser(), studentId));
    }

    [HttpPost]
    [Authorize()]
    public async Task<IActionResult> PostAsync([FromBody] StudentFileCreateModel payload)
    {
        await _studentFileService.AddAsync(GetAuthenticatedUser(), payload.MapToStudentFile());
        return Ok();
    }

    [HttpDelete()]
    [Authorize()]
    public async Task<IActionResult> DeleteAsync([FromBody] StudentFileRemoveModel payload)
    {
        await _studentFileService.RemoveAsync(GetAuthenticatedUser(), payload.MapToStudentFile());
        return Ok();
    }
}
