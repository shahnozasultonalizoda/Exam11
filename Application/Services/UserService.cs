using Application.DTOs.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Enums;

namespace Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Result<List<GetUserDto>>> GetAllAsync()
    {
        var users = await userRepository.GetAllAsync();

        return Result<List<GetUserDto>>.Ok(users.Select(u => new GetUserDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role.ToString(),
            CreatedAt = u.CreatedAt
        }).ToList());
    }

    public async Task<Result<GetUserDto>> GetByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result<GetUserDto>.Fail("User not found", ErrorType.NotFound);
        }

        return Result<GetUserDto>.Ok(new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        });
    }

    public async Task<Result<GetUserDto>> UpdateRoleAsync(int id, UpdateUserDto dto)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result<GetUserDto>.Fail("User not found", ErrorType.NotFound);
        }

        if (!Enum.TryParse<UserRole>(dto.Role, out var role))
        {
            return Result<GetUserDto>.Fail("Invalid role", ErrorType.Validation);
        }

        user.Role = role;
        await userRepository.UpdateAsync(user);

        return Result<GetUserDto>.Ok(new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return Result<bool>.Fail("User not found", ErrorType.NotFound);
        }

        await userRepository.DeleteAsync(user);
        return Result<bool>.Ok(true);
    }

   

}