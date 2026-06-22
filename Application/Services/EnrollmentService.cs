using Application.DTOs.Enrollment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class EnrollmentService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository) : IEnrollmentService
{
    public async Task<Result<GetEnrollmentDto>> EnrollAsync(int studentId, CreateEnrollmentDto dto)
    {
        var course = await courseRepository.GetByIdAsync(dto.CourseId);
        if (course is null)
        {
            return Result<GetEnrollmentDto>.Fail("Course not found", ErrorType.NotFound);
        }
        if (!course.IsPublished)
        {
            return Result<GetEnrollmentDto>.Fail("Course is not published", ErrorType.Validation);
        }

        var existing = await enrollmentRepository.GetByStudentAndCourseAsync(studentId, dto.CourseId);
        if (existing is not null)
        {
            return Result<GetEnrollmentDto>.Fail("Already enrolled in this course", ErrorType.Conflict);
        }

        var enrollment = new Enrollment
        {
            CourseId = dto.CourseId,
            StudentId = studentId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            ProgressPercent = 0
        };

        await enrollmentRepository.CreateAsync(enrollment);

        return Result<GetEnrollmentDto>.Ok(new GetEnrollmentDto
        {
            Id = enrollment.Id,
            CourseTitle = course.Title,
            StudentName = "",
            EnrolledAt = enrollment.EnrolledAt,
            Status = enrollment.Status.ToString(),
            ProgressPercent = enrollment.ProgressPercent
        });
    }

    public async Task<Result<List<GetEnrollmentDto>>> GetByStudentIdAsync(int studentId)
    {
        var enrollments = await enrollmentRepository.GetByStudentIdAsync(studentId);

        return Result<List<GetEnrollmentDto>>.Ok(enrollments.Select(e => new GetEnrollmentDto
        {
            Id = e.Id,
            CourseTitle = e.Course.Title,
            StudentName = e.Student.User.FullName,
            EnrolledAt = e.EnrolledAt,
            CompletedAt = e.CompletedAt,
            Status = e.Status.ToString(),
            ProgressPercent = e.ProgressPercent
        }).ToList());
    }

    public async Task<Result<GetEnrollmentDto>> UpdateProgressAsync(int enrollmentId, UpdateProgressDto dto, int studentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment is null)
        {
            return Result<GetEnrollmentDto>.Fail("Enrollment not found", ErrorType.NotFound);
        }
        if (enrollment.StudentId != studentId)
        {
            return Result<GetEnrollmentDto>.Fail("Access denied", ErrorType.Unauthorized);
        }
        if (dto.ProgressPercent < 0 || dto.ProgressPercent > 100)
        {
            return Result<GetEnrollmentDto>.Fail("Progress must be between 0 and 100", ErrorType.Validation);
        }

        enrollment.ProgressPercent = dto.ProgressPercent;

        if (dto.ProgressPercent == 100)
        {
            enrollment.Status = EnrollmentStatus.Completed;
            enrollment.CompletedAt = DateTime.UtcNow;
        }

        await enrollmentRepository.UpdateAsync(enrollment);

        return Result<GetEnrollmentDto>.Ok(new GetEnrollmentDto
        {
            Id = enrollment.Id,
            CourseTitle = enrollment.Course.Title,
            StudentName = enrollment.Student.User.FullName,
            EnrolledAt = enrollment.EnrolledAt,
            CompletedAt = enrollment.CompletedAt,
            Status = enrollment.Status.ToString(),
            ProgressPercent = enrollment.ProgressPercent
        });
    }

    public async Task<Result<bool>> CancelAsync(int enrollmentId, int studentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment is null)
        {
            return Result<bool>.Fail("Enrollment not found", ErrorType.NotFound);
        }
        if (enrollment.StudentId != studentId)
        {
            return Result<bool>.Fail("Access denied", ErrorType.Unauthorized);
        }

        enrollment.Status = EnrollmentStatus.Cancelled;
        await enrollmentRepository.UpdateAsync(enrollment);
        return Result<bool>.Ok(true);
    }
}