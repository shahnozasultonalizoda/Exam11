namespace Application.DTOs.DashboardDto;

public class TopCourseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public int EnrollmentsCount { get; set; }
    public double AverageRating { get; set; }
}
