using Aplication.Results;
using Application.DTOs.Enrollment;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IEnrollmentService
{
    Task<Result<EnrollmentDto>> EnrollAsync(string studentId, CreateEnrollmentDto dto);
    Task<Result<bool>> CancelAsync(int enrollmentId, string studentId);
    Task<Result<EnrollmentDto>> UpdateProgressAsync(int enrollmentId, UpdateProgressDto dto, string studentId);
    Task<Result<List<EnrollmentDto>>> GetMyEnrollmentsAsync(string studentId);
    Task<Result<PagedResult<EnrollmentDto>>> GetAllAsync(PagedRequest request);
}

