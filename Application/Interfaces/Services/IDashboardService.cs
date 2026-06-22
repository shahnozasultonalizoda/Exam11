using Application.DTOs.DashboardDto;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IDashboardService
{
    Task<Result<DashboardSummaryDto>> GetSummaryAsync();
    Task<Result<List<TopCourseDto>>> GetTopCoursesAsync();
}
