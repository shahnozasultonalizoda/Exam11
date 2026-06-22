using Domain.Entities;

 
namespace Application.Interfaces.Repositories;
 
public interface ICourseRepository
{
    Task<IQueryable<Course>> GetAll();
    Task<Course?> GetByIdAsync(int id);
    Task<int> CreateAsync(Course course);
    Task<int> UpdateAsync(Course course);
    Task<int> DeleteAsync(Course course);
}