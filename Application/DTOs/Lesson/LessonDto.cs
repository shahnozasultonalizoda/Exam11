namespace Application.DTOs.Lesson;

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public int CourseId { get; set; }
    public DateTime CreatedAt { get; set; }
}
