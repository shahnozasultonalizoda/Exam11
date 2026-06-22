using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetByStudentIdAsync(int studentId);
    Task<Enrollment?> GetByIdAsync(int id);
    Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<Enrollment> CreateAsync(Enrollment enrollment);
    Task<Enrollment> UpdateAsync(Enrollment enrollment);
    Task<List<TopEnrollment>> GetTopCoursesAsync(int count);

}

public class TopEnrollment
{
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public int EnrollmentsCount { get; set; }
}
