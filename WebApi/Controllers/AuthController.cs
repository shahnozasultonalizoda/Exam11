using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register( RegisterDto dto)
    {
        var result = await authService.RegisterAsync(dto);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == ErrorType.Conflict)
                return Conflict(result.Error);
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login( LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        if (!result.IsSuccess)
        {
            return Unauthorized(result.Error);
        }
        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword( ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await authService.ChangePasswordAsync(userId, dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Password changed successfully" });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var email = User.FindFirstValue(ClaimTypes.Email)!;
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        return Ok(new UserDto
        {
            Id = userId,
            Email = email,
            Role = role
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword( ForgotPasswordDto dto)
    {
        await authService.ForgotPasswordAsync(dto);
        return Ok(new { message = "If email exists, reset token has been sent" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword( ResetPasswordDto dto)
    {
        var result = await authService.ResetPasswordAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(new { message = "Password reset successfully" });
    }
}