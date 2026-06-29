using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.Lesson;

public class CreateLessonViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";
    public string? Content { get; set; }

    [Required(ErrorMessage = "Order is required")]
    public int Order { get; set; }

    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }
}
