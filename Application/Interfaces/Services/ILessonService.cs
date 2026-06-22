using Application.DTOs.Lesson;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ILessonService
{
    Task<Result<List<GetLessonDto>>> GetByCourseIdAsync(int courseId);
    Task<Result<GetLessonDto>> GetByIdAsync(int id);
    Task<Result<GetLessonDto>> CreateAsync(CreateLessonDto dto, int instructorId);
    Task<Result<GetLessonDto>> UpdateAsync(int id, UpdateLessonDto dto, int instructorId);
    Task<Result<bool>> DeleteAsync(int id, int instructorId);
}
