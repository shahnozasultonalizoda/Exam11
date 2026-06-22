using Domain.Enums;

namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? ThumbnailPath { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int InstructorId { get; set; }
    public User Instructor { get; set; } = null!;

    public List<Lesson> Lessons { get; set; } = new();
    public List<Enrollment> Enrollments { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}