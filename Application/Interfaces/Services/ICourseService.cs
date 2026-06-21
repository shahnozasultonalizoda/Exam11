using Aplication.Results;
using Application.DTOs.Course;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ICourseService
{
    Task<Result<PagedResult<CourseDto>>> GetCoursesAsync(CourseFilterDto filter);
    Task<Result<CourseDto>> GetByIdAsync(int id);
    Task<Result<CourseDto>> CreateAsync(CreateCourseDto dto, string instructorId);
    Task<Result<CourseDto>> UpdateAsync(int id, UpdateCourseDto dto, string currentUserId, bool isAdmin);
    Task<Result<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin);
    Task<Result<CourseDto>> TogglePublishAsync(int id, string currentUserId);
    // Task<Result<string>> UploadThumbnailAsync(int id, IFormFile file, string currentUserId);
}
