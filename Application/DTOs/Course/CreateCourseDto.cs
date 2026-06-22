using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Course;

public class CreateCourseDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;
    [Required] 
    public string Description { get; set; } = null!;
    [Range(0, double.MaxValue)] 
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    [Required] 
    public int CategoryId { get; set; } 
}
