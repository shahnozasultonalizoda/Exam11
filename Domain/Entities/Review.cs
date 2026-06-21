namespace Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }  // 1–5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // StudentId — это UserId из Identity
    public string StudentId { get; set; } = string.Empty;
    public ApplicationUser Student { get; set; } = null!;
}