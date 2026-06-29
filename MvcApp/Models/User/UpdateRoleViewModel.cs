using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.User;

public class UpdateRoleViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = "";
}
