using Application.DTOs.ReviewDTOs;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IReviewService
{
    Task<Result<List<GetReviewDto>>> GetByCourseIdAsync(int courseId);
    Task<Result<GetReviewDto>> CreateAsync(int courseId, CreateReviewDto dto, int studentId);
    Task<Result<GetReviewDto>> UpdateAsync(int id, UpdateReviewDto dto, int studentId);
    Task<Result<bool>> DeleteAsync(int id, int userId, string role);
}