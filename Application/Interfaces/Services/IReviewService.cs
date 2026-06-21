using Application.DTOs.Enrollment;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IReviewService
{
    Task<Result<List<ReviewDto>>> GetByCourseIdAsync(int courseId);
    Task<Result<ReviewDto>> CreateAsync(int courseId, CreateReviewDto dto, string studentId);
    Task<Result<ReviewDto>> UpdateAsync(int courseId, int reviewId, UpdateReviewDto dto, string studentId, bool isAdmin);
    Task<Result<bool>> DeleteAsync(int courseId, int reviewId, string studentId, bool isAdmin);
}
