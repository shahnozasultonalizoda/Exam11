using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetByCourseIdAsync(int courseId);
    Task<Review?> GetByIdAsync(int id);
    Task<Review?> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<Review> CreateAsync(Review review);
    Task DeleteAsync(Review review);
}
