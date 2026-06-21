using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Lesson;

public class UpdateLessonDto
{
    [MaxLength(300)] 
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int? OrderIndex { get; set; }
    [Range(0, 600)] 
    public int? DurationMinutes { get; set; }
}
