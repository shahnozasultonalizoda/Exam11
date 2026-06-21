using Domain.Enums;

namespace Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public EnrollmentStatus Status { get; set; }
    public int ProgressPercent { get; set; }  // 0–100

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // StudentId — это просто UserId из Identity (не отдельная сущность!)
    public string StudentId { get; set; } = string.Empty;
    public ApplicationUser Student { get; set; } = null!;
}