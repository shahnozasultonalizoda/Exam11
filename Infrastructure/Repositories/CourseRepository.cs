using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
    public async Task<IQueryable<Course>> GetAll()
    {
        return context.Courses.AsQueryable();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await context.Courses.FindAsync(id);
    }

    public async Task<int> CreateAsync(Course course)
    {
        context.Courses.Add(course);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Course course)
    {
        context.Courses.Update(course);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Course course)
    {
        context.Courses.Remove(course);
        return await context.SaveChangesAsync();
    }
}