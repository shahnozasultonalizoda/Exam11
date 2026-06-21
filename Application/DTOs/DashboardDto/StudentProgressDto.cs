namespace Application.DTOs.DashboardDto;

public class StudentProgressDto
{
    public string StudentId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public int CompletedCourses { get; set; }
    public int ActiveEnrollments { get; set; }
    public double AverageProgress { get; set; }
}
