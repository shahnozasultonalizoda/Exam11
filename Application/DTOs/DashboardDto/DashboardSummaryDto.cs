namespace Application.DTOs.DashboardDto;

public class DashboardSummaryDto
{
    public int TotalCourses { get; set; }
    public int TotalStudents { get; set; }
    public int TotalEnrollments { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<TopCourseDto> TopCourses { get; set; } = new();
}
