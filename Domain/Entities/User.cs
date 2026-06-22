using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpiry { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public Student? Student { get; set; }
    public List<Course> Courses { get; set; } = [];
}