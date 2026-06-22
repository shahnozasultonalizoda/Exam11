using Application.DTOs.DashboardDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class DashboardService(ICourseRepository courseRepository, IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository) : IDashboardService
{
    public async Task<Result<DashboardSummaryDto>> GetSummaryAsync()
    {
        var courses = await courseRepository.GetAll();
        var students = await studentRepository.GetAllAsync();
        var topCourses = await enrollmentRepository.GetTopCoursesAsync(5);

        return Result<DashboardSummaryDto>.Ok(new DashboardSummaryDto
        {
            TotalCourses = await courses.CountAsync(),
            TotalStudents = students.Count,
            TotalEnrollments = await courses.SumAsync(c => c.Enrollments.Count),
            TotalRevenue = await courses.Where(c => c.IsPublished).SumAsync(c => c.Price * c.Enrollments.Count),
            TopCourses = topCourses.Select(t => new TopCourseDto
            {
                CourseId = t.CourseId,
                Title = t.Title,
                EnrollmentsCount = t.EnrollmentsCount
            }).ToList()
        });
    }

    public async Task<Result<List<TopCourseDto>>> GetTopCoursesAsync()
    {
        var top = await enrollmentRepository.GetTopCoursesAsync(5);

        return Result<List<TopCourseDto>>.Ok(top.Select(t => new TopCourseDto
        {
            CourseId = t.CourseId,
            Title = t.Title,
            EnrollmentsCount = t.EnrollmentsCount
        }).ToList());
    }
}