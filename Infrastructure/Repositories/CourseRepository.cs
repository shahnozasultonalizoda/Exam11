using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;
 
    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }
 
    public IQueryable<Course> GetAll()
    {
        return _context.Courses.AsQueryable();
    }
 
    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FindAsync(id);
    }
 
    public async Task<Course?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Courses.FindAsync(id);
    }
 
    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }
 
    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Course course)
    {
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }
 
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Courses.FindAsync(id) is not null;
    }
}
