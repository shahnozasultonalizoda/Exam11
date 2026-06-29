using Application.DTOs.ReviewDTOs;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/courses/{courseId}/reviews")]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByCourseId(int courseId)
    {
        var result = await reviewService.GetByCourseIdAsync(courseId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> Create(int courseId, [FromBody] CreateReviewDto dto)
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await reviewService.CreateAsync(courseId, dto, studentId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.Conflict)
                return Conflict(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Student")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int courseId, int id, [FromBody] UpdateReviewDto dto)
    {
        var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await reviewService.UpdateAsync(id, dto, studentId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int courseId, int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        var result = await reviewService.DeleteAsync(id, userId, role);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Review deleted successfully" });
    }
}