// WebApi/Controllers/LessonsController.cs
using Application.DTOs.Lesson;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/lessons")]
[Authorize]
public class LessonsController(ILessonService lessonService) : ControllerBase
{
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourseId(int courseId)
    {
        var result = await lessonService.GetByCourseIdAsync(courseId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await lessonService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await lessonService.CreateAsync(dto, instructorId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await lessonService.UpdateAsync(id, dto, instructorId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Instructor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await lessonService.DeleteAsync(id, instructorId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Lesson deleted successfully" });
    }
}