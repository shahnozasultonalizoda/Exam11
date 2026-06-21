using Application.DTOs.Lesson;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ILessonService
{
    Task<Result<List<LessonDto>>> GetByCourseIdAsync(int courseId, string currentUserId, bool isAdmin);
    Task<Result<LessonDto>> GetByIdAsync(int courseId, int lessonId, string currentUserId, bool isAdmin);
    Task<Result<LessonDto>> CreateAsync(int courseId, CreateLessonDto dto, string currentUserId);
    Task<Result<LessonDto>> UpdateAsync(int courseId, int lessonId, UpdateLessonDto dto, string currentUserId, bool isAdmin);
    Task<Result<bool>> DeleteAsync(int courseId, int lessonId, string currentUserId, bool isAdmin);
}
