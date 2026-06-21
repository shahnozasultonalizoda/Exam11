using Domain.Entities;

 
namespace Application.Interfaces.Repositories;
 
public interface ICourseRepository
{
    IQueryable<Course> GetAll();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
    Task<bool> ExistsAsync(int id);
}