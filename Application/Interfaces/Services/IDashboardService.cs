using Application.DTOs.DashboardDto;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IDashboardService
{
    Task<Result<DashboardSummaryDto>> GetSummaryAsync();
    Task<Result<List<TopCourseDto>>> GetTopCoursesAsync();
    Task<Result<List<MonthlyEnrollmentDto>>> GetEnrollmentsByMonthAsync();
    Task<Result<List<CategoryRevenueDto>>> GetRevenueByCategoryAsync();
    Task<Result<List<CompletionRateDto>>> GetCompletionRateAsync();
    Task<Result<InstructorStatsDto>> GetInstructorStatsAsync(string instructorId, string currentUserId, bool isAdmin);
    Task<Result<StudentsProgressSummaryDto>> GetStudentsProgressAsync();
    Task<Result<RatingsDistributionDto>> GetRatingsDistributionAsync();
}
