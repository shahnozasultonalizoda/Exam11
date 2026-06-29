namespace MvcApp.Models;

public class LoginResponseViewModel
{
    public string Token { get; set; } = "";
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
}
