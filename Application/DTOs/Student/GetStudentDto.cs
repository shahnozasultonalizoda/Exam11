namespace Application.DTOs.Student;

public class GetStudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Bio { get; set; }
}
