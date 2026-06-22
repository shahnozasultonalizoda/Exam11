using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByUserIdAsync(int userId);
    Task<Student> UpdateAsync(Student student);
}
