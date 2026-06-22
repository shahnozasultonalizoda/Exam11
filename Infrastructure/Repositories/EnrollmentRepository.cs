using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EnrollmentRepository(AppDbContext context) : IEnrollmentRepository
{
    public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        return await context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await context.Enrollments.FindAsync(id);
    }

    public async Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        return await context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }

    public async Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
    {
        context.Enrollments.Update(enrollment);
        await context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<List<TopEnrollment>> GetTopCoursesAsync(int count)
    {
        return await context.Enrollments
            .GroupBy(e => new { e.CourseId, e.Course.Title })
            .Select(g => new TopEnrollment
            {
                CourseId = g.Key.CourseId,
                Title = g.Key.Title,
                EnrollmentsCount = g.Count()
            })
            .OrderByDescending(x => x.EnrollmentsCount)
            .Take(count)
            .ToListAsync();
    }
}