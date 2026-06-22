namespace Domain.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Content { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}