using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;
 
    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }
 
    public IQueryable<Enrollment> GetAll()
    {
        return _context.Enrollments.AsQueryable();
    }
 
    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments.FindAsync(id);
    }
 
    public async Task<Enrollment?> GetByStudentAndCourseAsync(string studentId, int courseId)
    {
        return await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }
 
    public async Task<List<Enrollment>> GetByStudentIdAsync(string studentId)
    {
        return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }
 
    public async Task<bool> IsEnrolledAsync(string studentId, int courseId)
    {
        return await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }
 
    public async Task AddAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
    }
 
    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }
}
