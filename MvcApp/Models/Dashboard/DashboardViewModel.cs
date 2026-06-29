namespace MvcApp.Models.Dashboard;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }
    public int TotalStudents { get; set; }
    public int TotalEnrollments { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<TopCourseItem> TopCourses { get; set; } = new();
}

public class TopCourseItem
{
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public int EnrollmentsCount { get; set; }
}
