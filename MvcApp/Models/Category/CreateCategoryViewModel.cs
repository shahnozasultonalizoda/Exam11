using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.Category;

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}
