namespace MvcApp.Models.Lesson;

public class LessonViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Content { get; set; }
    public int Order { get; set; }
    public int CourseId { get; set; }
}
