using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Enrollment;

public class EnrollmentDto
{
    public int Id { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public EnrollmentStatus Status { get; set; }
    public int ProgressPercent { get; set; }
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string? CourseThumbnailUrl { get; set; }
    public string StudentId { get; set; } = null!;
    public string StudentName { get; set; } = null!;
}


public class CreateEnrollmentDto
{
    [Required] public int CourseId { get; set; }
}
 
public class UpdateProgressDto
{
    [Range(0, 100)] public int ProgressPercent { get; set; }
}
 
public class ReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string StudentName { get; set; } = null!;
    public string StudentId { get; set; } = null!;
}
 
public class CreateReviewDto
{
    [Range(1, 5)] 
    public int Rating { get; set; }
    [Required, MaxLength(2000)] 
    public string Comment { get; set; } = null!;
}
 
public class UpdateReviewDto
{
    [Range(1, 5)] 
    public int? Rating { get; set; }
    [MaxLength(2000)] 
    public string? Comment { get; set; }
}
