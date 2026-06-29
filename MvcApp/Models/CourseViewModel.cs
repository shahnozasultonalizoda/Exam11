using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class CourseViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? ThumbnailPath { get; set; }
    public decimal Price { get; set; }
    public string Level { get; set; } = "";
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CategoryName { get; set; } = "";
    public string InstructorName { get; set; } = "";
    public int LessonsCount { get; set; }
    public double AverageRating { get; set; }
    public int CategoryId { get; set; }
}

public class CreateCourseViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = "";

    [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Level is required")]
    public string Level { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
}

public class UpdateCourseViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = "";

    [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Level is required")]
    public string Level { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
}
