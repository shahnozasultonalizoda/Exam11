using Application.DTOs.Auth;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<Result<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto dto);
}
