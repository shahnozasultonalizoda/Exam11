using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Course;

public class UpdateCourseDto
{
    [MaxLength(200)]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Range(0, double.MaxValue)] 
    public decimal? Price { get; set; }
    public CourseLevel? Level { get; set; }
    public int? CategoryId { get; set; }
}
