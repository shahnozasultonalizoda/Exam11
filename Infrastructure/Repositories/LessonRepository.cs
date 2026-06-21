using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Domain.Entities;


namespace OnlineCourses.Infrastructure.Repositories;
 
public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;
 
    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
 
    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons.FindAsync(id);
    }
 
    public async Task<List<Lesson>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .ToListAsync();
    }
 
    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }
 
    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Lesson lesson)
    {
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }
}
 
public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
 
    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }
 
    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }
 
    public async Task<List<Review>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Reviews
            .Where(r => r.CourseId == courseId)
            .ToListAsync();
    }
 
    public async Task<Review?> GetByStudentAndCourseAsync(string studentId, int courseId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);
    }
 
    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }
 
    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Review review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}
 
public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
 
    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }
 
    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
 
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
}
 