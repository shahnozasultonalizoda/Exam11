using Aplication.Results;
using Application.DTOs.Enrollment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class EnrollmentService(
    IEnrollmentRepository enrollmentRepository,
    ICourseRepository courseRepository) : IEnrollmentService
{
    public async Task<Result<EnrollmentDto>> EnrollAsync(string studentId, CreateEnrollmentDto dto)
    {
        var course = await courseRepository.GetByIdAsync(dto.CourseId);
        if (course is null)
            return Result<EnrollmentDto>.Fail("Курс не найден.", ErrorType.NotFound);

        if (!course.IsPublished)
            return Result<EnrollmentDto>.Fail("Курс не опубликован.", ErrorType.Validation);

        var alreadyEnrolled = await enrollmentRepository.IsEnrolledAsync(studentId, dto.CourseId);
        if (alreadyEnrolled)
            return Result<EnrollmentDto>.Fail("Вы уже записаны на этот курс.", ErrorType.Conflict);

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = dto.CourseId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            ProgressPercent = 0
        };

        await enrollmentRepository.AddAsync(enrollment);

        var created = await enrollmentRepository.GetAll()
            .FirstOrDefaultAsync(e => e.Id == enrollment.Id);

        return Result<EnrollmentDto>.Ok(MapToDto(created!));
    }

    public async Task<Result<bool>> CancelAsync(int enrollmentId, string studentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment is null)
            return Result<bool>.Fail("Запись не найдена.", ErrorType.NotFound);

        if (enrollment.StudentId != studentId)
            return Result<bool>.Fail("Нет доступа.", ErrorType.Unauthorized);

        await enrollmentRepository.DeleteAsync(enrollment);
        return Result<bool>.Ok(true);
    }

    public async Task<Result<EnrollmentDto>> UpdateProgressAsync(int enrollmentId, UpdateProgressDto dto, string studentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment is null)
            return Result<EnrollmentDto>.Fail("Запись не найдена.", ErrorType.NotFound);

        if (enrollment.StudentId != studentId)
            return Result<EnrollmentDto>.Fail("Нет доступа.", ErrorType.Unauthorized);

        enrollment.ProgressPercent = dto.ProgressPercent;

        if (dto.ProgressPercent == 100)
        {
            enrollment.Status = EnrollmentStatus.Completed;
            enrollment.CompletedAt = DateTime.UtcNow;
        }

        await enrollmentRepository.UpdateAsync(enrollment);

        var updated = await enrollmentRepository.GetAll()
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        return Result<EnrollmentDto>.Ok(MapToDto(updated!));
    }

    public async Task<Result<List<EnrollmentDto>>> GetMyEnrollmentsAsync(string studentId)
    {
        var enrollments = await enrollmentRepository.GetAll()
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

        return Result<List<EnrollmentDto>>.Ok(enrollments.Select(MapToDto).ToList());
    }

    public async Task<Result<PagedResult<EnrollmentDto>>> GetAllAsync(PagedRequest request)
    {
        var query = enrollmentRepository.GetAll();

        var totalCount = await query.CountAsync();
        var pageSize = Math.Clamp(request.PageSize, 1, 50);
        var page = Math.Max(request.Page, 1);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Result<PagedResult<EnrollmentDto>>.Ok(
            PagedResult<EnrollmentDto>.Ok(items.Select(MapToDto).ToList(), totalCount, page, pageSize));
    }

    private static EnrollmentDto MapToDto(Enrollment e) => new()
    {
        Id = e.Id,
        EnrolledAt = e.EnrolledAt,
        CompletedAt = e.CompletedAt,
        Status = e.Status,
        ProgressPercent = e.ProgressPercent,
        CourseId = e.CourseId,
        CourseTitle = e.Course?.Title ?? string.Empty,
        CourseThumbnailUrl = e.Course?.ThumbnailPath,
        StudentId = e.StudentId,
        StudentName = e.Student?.FullName ?? string.Empty
    };
}