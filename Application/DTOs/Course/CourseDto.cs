using Domain.Enums;

namespace Application.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public string InstructorId { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public int EnrollmentCount { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
