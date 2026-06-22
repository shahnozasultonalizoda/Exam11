// WebApi/Controllers/CoursesController.cs
using Application.DTOs.Course;
using Application.DTOs.CourseDTOs;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] CourseFilterDto filter)
    {
        var result = await courseService.GetFilteredAsync(filter);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await courseService.GetByIdAsync(id);
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
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await courseService.CreateAsync(dto, instructorId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await courseService.UpdateAsync(id, dto, instructorId);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        var result = await courseService.DeleteAsync(id, instructorId, role);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Course deleted successfully" });
    }

   
}