using Application.DTOs.User;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await userService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await userService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await userService.UpdateRoleAsync(id, dto);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await userService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.NotFound)
                return NotFound(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { message = "User deleted successfully" });
    }
}