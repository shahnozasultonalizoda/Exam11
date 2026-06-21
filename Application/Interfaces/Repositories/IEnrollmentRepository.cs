using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEnrollmentRepository
{
    IQueryable<Enrollment> GetAll();
    Task<Enrollment?> GetByIdAsync(int id);
    Task<Enrollment?> GetByStudentAndCourseAsync(string studentId, int courseId);
    Task<List<Enrollment>> GetByStudentIdAsync(string studentId);
    Task<bool> IsEnrolledAsync(string studentId, int courseId);
    Task AddAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
    Task DeleteAsync(Enrollment enrollment);
}
