using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id);
    Task<List<Review>> GetByCourseIdAsync(int courseId);
    Task<Review?> GetByStudentAndCourseAsync(string studentId, int courseId);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
}
