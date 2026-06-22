using Application.DTOs.Enrollment;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IEnrollmentService
{
    Task<Result<GetEnrollmentDto>> EnrollAsync(int studentId, CreateEnrollmentDto dto);
    Task<Result<List<GetEnrollmentDto>>> GetByStudentIdAsync(int studentId);
    Task<Result<GetEnrollmentDto>> UpdateProgressAsync(int enrollmentId, UpdateProgressDto dto, int studentId);
    Task<Result<bool>> CancelAsync(int enrollmentId, int studentId);
}
