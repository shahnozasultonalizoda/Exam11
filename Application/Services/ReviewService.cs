using Application.DTOs.ReviewDTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ReviewService(IReviewRepository reviewRepository, IEnrollmentRepository enrollmentRepository) : IReviewService
{
    public async Task<Result<List<GetReviewDto>>> GetByCourseIdAsync(int courseId)
    {
        var reviews = await reviewRepository.GetByCourseIdAsync(courseId);

        return Result<List<GetReviewDto>>.Ok(reviews.Select(r => new GetReviewDto
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            StudentId = r.StudentId,
            CourseId = r.CourseId
        }).ToList());
    }

    public async Task<Result<GetReviewDto>> CreateAsync(int courseId, CreateReviewDto dto, int studentId)
    {
        var enrollment = await enrollmentRepository.GetByStudentAndCourseAsync(studentId, courseId);
        if (enrollment is null || enrollment.Status != EnrollmentStatus.Active)
        {
            return Result<GetReviewDto>.Fail("You must be enrolled in this course", ErrorType.Validation);
        }

        var existing = await reviewRepository.GetByStudentAndCourseAsync(studentId, courseId);
        if (existing is not null)
        {
            return Result<GetReviewDto>.Fail("You already reviewed this course", ErrorType.Conflict);
        }

        if (dto.Rating < 1 || dto.Rating > 5)
        {
            return Result<GetReviewDto>.Fail("Rating must be between 1 and 5", ErrorType.Validation);
        }

        var review = new Review
        {
            Rating = dto.Rating,
            Comment = dto.Comment,
            CourseId = courseId,
            StudentId = studentId,
            CreatedAt = DateTime.UtcNow
        };

        await reviewRepository.CreateAsync(review);

        return Result<GetReviewDto>.Ok(new GetReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            StudentId = review.StudentId,
            CourseId = review.CourseId
        });
    }

    public async Task<Result<GetReviewDto>> UpdateAsync(int id, UpdateReviewDto dto, int studentId)
    {
        var review = await reviewRepository.GetByIdAsync(id);
        if (review is null)
        {
            return Result<GetReviewDto>.Fail("Review not found", ErrorType.NotFound);
        }
        if (review.StudentId != studentId)
        {
            return Result<GetReviewDto>.Fail("Access denied", ErrorType.Unauthorized);
        }
        if (dto.Rating < 1 || dto.Rating > 5)
        {
            return Result<GetReviewDto>.Fail("Rating must be between 1 and 5", ErrorType.Validation);
        }

        review.Rating = dto.Rating;
        review.Comment = dto.Comment;

        await reviewRepository.CreateAsync(review);

        return Result<GetReviewDto>.Ok(new GetReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            StudentId = review.StudentId,
            CourseId = review.CourseId
        });
    }

    public async Task<Result<bool>> DeleteAsync(int id, int userId, string role)
    {
        var review = await reviewRepository.GetByIdAsync(id);
        if (review is null)
        {
            return Result<bool>.Fail("Review not found", ErrorType.NotFound);
        }
        if (role != "Admin" && review.StudentId != userId)
        {
            return Result<bool>.Fail("Access denied", ErrorType.Unauthorized);
        }

        await reviewRepository.DeleteAsync(review);
        return Result<bool>.Ok(true);
    }
}