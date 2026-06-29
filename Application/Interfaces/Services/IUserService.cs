using Application.DTOs.User;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<List<GetUserDto>>> GetAllAsync();
    Task<Result<GetUserDto>> GetByIdAsync(int id);
    Task<Result<GetUserDto>> UpdateRoleAsync(int id, UpdateUserDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}