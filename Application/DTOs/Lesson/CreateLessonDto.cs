using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Lesson;

public class CreateLessonDto
{
    [Required, MaxLength(300)] 
    public string Title { get; set; } = null!;
    [Required] 
    public string Content { get; set; } = null!;
    public int Order { get; set; }
    public int CourseId { get; set; }
}
