using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}
