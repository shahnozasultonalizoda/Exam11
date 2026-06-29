using Application.DTOs.DashboardDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Application.Services;

public class DashboardService(
    ICourseRepository courseRepository,
    IStudentRepository studentRepository,
    IEnrollmentRepository enrollmentRepository,
    IDistributedCache cache) : IDashboardService
{
    public async Task<Result<DashboardSummaryDto>> GetSummaryAsync()
    {
        const string key = "dashboard:summary";

        var cached = await cache.GetStringAsync(key);
        if (cached is not null)
        {
            return Result<DashboardSummaryDto>.Ok(
                JsonSerializer.Deserialize<DashboardSummaryDto>(cached)!);
        }

        var courses = await courseRepository.GetAll();
        var students = await studentRepository.GetAllAsync();
        var topCourses = await enrollmentRepository.GetTopCoursesAsync(5);

        var summary = new DashboardSummaryDto
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
        };

        await cache.SetStringAsync(key, JsonSerializer.Serialize(summary),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        return Result<DashboardSummaryDto>.Ok(summary);
    }

    public async Task<Result<List<TopCourseDto>>> GetTopCoursesAsync()
    {
        const string key = "dashboard:top_courses";

        var cached = await cache.GetStringAsync(key);
        if (cached is not null)
        {
            return Result<List<TopCourseDto>>.Ok(
                JsonSerializer.Deserialize<List<TopCourseDto>>(cached)!);
        }

        var top = await enrollmentRepository.GetTopCoursesAsync(5);
        var result = top.Select(t => new TopCourseDto
        {
            CourseId = t.CourseId,
            Title = t.Title,
            EnrollmentsCount = t.EnrollmentsCount
        }).ToList();

        await cache.SetStringAsync(key, JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

        return Result<List<TopCourseDto>>.Ok(result);
    }


}