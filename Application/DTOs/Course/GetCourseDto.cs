
namespace Application.DTOs.Course;

public class GetCourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? ThumbnailPath { get; set; }
    public decimal Price { get; set; }
    public string Level { get; set; } = "";
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CategoryName { get; set; } = "";
    public string InstructorName { get; set; } = "";
    public int LessonsCount { get; set; }
    public double AverageRating { get; set; }
}
