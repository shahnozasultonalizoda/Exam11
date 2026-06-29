using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class LessonViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Content { get; set; }
    public int Order { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = "";
}

public class CreateLessonViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";
    public string? Content { get; set; }
    public int Order { get; set; }

    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }
}

public class UpdateLessonViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";
    public string? Content { get; set; }
    public int Order { get; set; }
}
