using Application.DTOs.Course;
using Application.DTOs.CourseDTOs;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ICourseService
{
    Task<Result<List<GetCourseDto>>> GetFilteredAsync(CourseFilterDto filter);
    Task<Result<GetCourseDto>> GetByIdAsync(int id);
    Task<Result<GetCourseDto>> CreateAsync(CreateCourseDto dto, int instructorId);
    Task<Result<GetCourseDto>> UpdateAsync(int id, UpdateCourseDto dto, int instructorId);
    Task<Result<bool>> DeleteAsync(int id, int instructorId, string role);
}
