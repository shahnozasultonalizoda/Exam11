using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Lesson;

public class UpdateLessonDto
{
    [MaxLength(300)] 
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Order { get; set; }
    
}
