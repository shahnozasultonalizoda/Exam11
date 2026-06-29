using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService , IEmailService emailService) : IAuthService
{
    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        if (!Enum.TryParse<UserRole>(dto.Role, out var role) || role == UserRole.Admin)
        {
            return Result<AuthResponseDto>.Fail("Role must be Student or Instructor", ErrorType.Validation);
        }

        var existing = await userRepository.GetByEmailAsync(dto.Email);
        if (existing is not null)
        {
            return Result<AuthResponseDto>.Fail("Email already in use", ErrorType.Conflict);
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        if (role == UserRole.Student)
        {
            user.Student = new Student();
        }

        await userRepository.CreateAsync(user);

        return Result<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Token = await jwtTokenService.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        });
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Result<AuthResponseDto>.Fail("Invalid email or password", ErrorType.Unauthorized);
        }

        return Result<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Token = await jwtTokenService.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        });
    }

    public async Task<Result<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return Result<bool>.Fail("User not found", ErrorType.NotFound);
        }
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            return Result<bool>.Fail("Current password is incorrect", ErrorType.Validation);
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await userRepository.UpdateAsync(user);
        return Result<bool>.Ok(true);
    }



    public async Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto dto)
{
    var user = await userRepository.GetByEmailAsync(dto.Email);
    if (user is null)
    {
        return Result<bool>.Ok(true);
    }

    user.ResetPasswordToken = Guid.NewGuid().ToString("N");
    user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(15);
    await userRepository.UpdateAsync(user);

    await emailService.SendEmailAsync(
        user.Email,
        "Reset Password",
        $"Your reset token: {user.ResetPasswordToken}"
    );

    return Result<bool>.Ok(true);
}

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return Result<bool>.Fail("Passwords do not match", ErrorType.Validation);
        }

        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user is null)
        {
            return Result<bool>.Fail("User not found", ErrorType.NotFound);
        }
        if (user.ResetPasswordToken != dto.Token || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
        {
            return Result<bool>.Fail("Invalid or expired token", ErrorType.Validation);
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.ResetPasswordToken = null;
        user.ResetPasswordTokenExpiry = null;
        await userRepository.UpdateAsync(user);
        return Result<bool>.Ok(true);
    }
}