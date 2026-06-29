namespace MvcApp.Models;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }
    public int TotalStudents { get; set; }
    public int TotalEnrollments { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<TopCourseViewModel> TopCourses { get; set; } = new();
}

public class TopCourseViewModel
{
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public int EnrollmentsCount { get; set; }
}
