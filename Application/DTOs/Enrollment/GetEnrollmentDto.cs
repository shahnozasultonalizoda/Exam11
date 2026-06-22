namespace Application.DTOs.Enrollment;

public class GetEnrollmentDto
{
    public int Id { get; set; }
    public string CourseTitle { get; set; } = "";
    public string StudentName { get; set; } = "";
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = "";
    public int ProgressPercent { get; set; }
}
