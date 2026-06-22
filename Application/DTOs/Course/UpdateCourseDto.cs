using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Course;

public class UpdateCourseDto
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string Level { get; set; } = "";
    public int CategoryId { get; set; }
}
