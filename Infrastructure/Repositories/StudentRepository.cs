using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public async Task<List<Student>> GetAllAsync()
    {
        return await context.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await context.Students.FindAsync(id);
    }

    public async Task<Student?> GetByUserIdAsync(int userId)
    {
        return await context.Students
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        context.Students.Update(student);
        await context.SaveChangesAsync();
        return student;
    }
}