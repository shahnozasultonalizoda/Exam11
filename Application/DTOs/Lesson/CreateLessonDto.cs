using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Lesson;

public class CreateLessonDto
{
    [Required, MaxLength(300)] 
    public string Title { get; set; } = null!;
    [Required] 
    public string Content { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int OrderIndex { get; set; }
    [Range(0, 600)] 
    public int DurationMinutes { get; set; }
}
