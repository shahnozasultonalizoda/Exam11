using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(int id);
    Task<List<Lesson>> GetByCourseIdAsync(int courseId);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Lesson lesson);
}
