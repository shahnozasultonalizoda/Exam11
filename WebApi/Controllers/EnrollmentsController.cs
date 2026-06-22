// WebApi/Controllers/EnrollmentsController.cs
using Application.DTOs.Enrollment;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/enrollments")]
[Authorize]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await enrollmentService.EnrollAsync(studentId, dto);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.Conflict)
                return Conflict(result.Error);
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await enrollmentService.GetByStudentIdAsync(studentId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Student")]
    [HttpPut("{id}/progress")]
    public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateProgressDto dto)
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await enrollmentService.UpdateProgressAsync(id, dto, studentId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Student")]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await enrollmentService.CancelAsync(id, studentId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Enrollment cancelled successfully" });
    }
}