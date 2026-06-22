using Application.DTOs.Course;
using Application.DTOs.CourseDTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CourseService(ICourseRepository courseRepository) : ICourseService
{
    public async Task<Result<List<GetCourseDto>>> GetFilteredAsync(CourseFilterDto filter)
    {
        var query = await courseRepository.GetAll();

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(c => c.CategoryId == filter.CategoryId);
        }
        if (filter.MinPrice.HasValue)
        {
            query = query.Where(c => c.Price >= filter.MinPrice);
        }
        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(c => c.Price <= filter.MaxPrice);
        }

        var items = await query
            .Select(c => new GetCourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ThumbnailPath = c.ThumbnailPath,
                Price = c.Price,
                Level = c.Level.ToString(),
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                CategoryName = c.Category.Name,
                InstructorName = c.Instructor.FullName,
                LessonsCount = c.Lessons.Count,
                AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
            }).ToListAsync();

        return Result<List<GetCourseDto>>.Ok(items);
    }

    public async Task<Result<GetCourseDto>> GetByIdAsync(int id)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
        {
            return Result<GetCourseDto>.Fail("Course not found", ErrorType.NotFound);
        }

        return Result<GetCourseDto>.Ok(new GetCourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ThumbnailPath = course.ThumbnailPath,
            Price = course.Price,
            Level = course.Level.ToString(),
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt,
            CategoryName = course.Category.Name,
            InstructorName = course.Instructor.FullName,
            LessonsCount = course.Lessons.Count,
            AverageRating = course.Reviews.Any() ? course.Reviews.Average(r => r.Rating) : 0
        });
    }

    public async Task<Result<GetCourseDto>> CreateAsync(CreateCourseDto dto, int instructorId)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Level = dto.Level,
            CategoryId = dto.CategoryId,
            InstructorId = instructorId,
            IsPublished = false,
            CreatedAt = DateTime.UtcNow
        };

        await courseRepository.CreateAsync(course);
        return await GetByIdAsync(course.Id);
    }

    public async Task<Result<GetCourseDto>> UpdateAsync(int id, UpdateCourseDto dto, int instructorId)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
        {
            return Result<GetCourseDto>.Fail("Course not found", ErrorType.NotFound);
        }
        if (course.InstructorId != instructorId)
        {
            return Result<GetCourseDto>.Fail("Access denied", ErrorType.Unauthorized);
        }

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Price = dto.Price;
        course.CategoryId = dto.CategoryId;

        await courseRepository.UpdateAsync(course);
        return await GetByIdAsync(course.Id);
    }

    public async Task<Result<bool>> DeleteAsync(int id, int instructorId, string role)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
        {
            return Result<bool>.Fail("Course not found", ErrorType.NotFound);
        }
        if (role != UserRole.Admin.ToString() && course.InstructorId != instructorId)
        {
            return Result<bool>.Fail("Access denied", ErrorType.Unauthorized);
        }

        await courseRepository.DeleteAsync(course);
        return Result<bool>.Ok(true);
    }

    

    

}