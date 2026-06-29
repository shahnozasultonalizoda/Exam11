using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.Course;

public class CreateCourseViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Level is required")]
    public string Level { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
}
