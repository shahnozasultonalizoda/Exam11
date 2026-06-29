namespace MvcApp.Models;

public class ReviewViewModel
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}
