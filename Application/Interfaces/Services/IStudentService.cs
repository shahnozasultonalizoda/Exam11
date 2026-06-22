using Application.DTOs.Student;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IStudentService
{
    Task<Result<List<GetStudentDto>>> GetAllAsync();
    Task<Result<GetStudentDto>> GetByIdAsync(int id);
    Task<Result<GetStudentDto>> UpdateAsync(int id, UpdateStudentDto dto, int currentUserId);
}
