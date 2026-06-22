namespace Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string? Bio { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<Enrollment> Enrollments { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}