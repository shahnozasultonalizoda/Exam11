using Aplication.Results;
using Application.DTOs.Auth;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<PagedResult<UserDto>>> GetAllAsync(PagedRequest request);
    Task<Result<UserDto>> GetByIdAsync(string id);
    Task<Result<bool>> ChangeRoleAsync(string userId, string newRole);
    Task<Result<bool>> DeleteAsync(string userId);
}
