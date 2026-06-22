using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    public async Task<List<Review>> GetByCourseIdAsync(int courseId)
    {
        return await context.Reviews
            .Where(r => r.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await context.Reviews.FindAsync(id);
    }

    public async Task<Review?> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        return await context.Reviews
            .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task DeleteAsync(Review review)
    {
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
    }
}