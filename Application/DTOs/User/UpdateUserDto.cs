namespace Application.DTOs.User;

public class UpdateUserDto
{
    public string? FullName { get; set; } 
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = "";
}