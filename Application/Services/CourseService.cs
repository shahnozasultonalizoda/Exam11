using Aplication.Results;
using Application.DTOs.Course;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace OnlineCourses.Application.Services;

public class CourseService(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository) : ICourseService
{
    public async Task<Result<PagedResult<CourseDto>>> GetCoursesAsync(CourseFilterDto filter)
    {
        var query = courseRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(c => c.Title.Contains(filter.Search) || c.Description.Contains(filter.Search));

        if (filter.CategoryId.HasValue)
            query = query.Where(c => c.CategoryId == filter.CategoryId);

        if (filter.Level.HasValue)
            query = query.Where(c => c.Level == filter.Level);

        if (filter.MinPrice.HasValue)
            query = query.Where(c => c.Price >= filter.MinPrice);

        if (filter.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= filter.MaxPrice);

        if (filter.IsPublished.HasValue)
            query = query.Where(c => c.IsPublished == filter.IsPublished);

        query = filter.SortBy switch
        {
            "price" => filter.SortDescending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
            "rating" => filter.SortDescending
                ? query.OrderByDescending(c => c.Reviews.Average(r => (double?)r.Rating))
                : query.OrderBy(c => c.Reviews.Average(r => (double?)r.Rating)),
            _ => filter.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
        };

        var pageSize = Math.Clamp(filter.PageSize, 1, 50);
        var page = Math.Max(filter.Page, 1);
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ThumbnailUrl = c.ThumbnailPath,
                Price = c.Price,
                Level = c.Level,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                CategoryId = c.CategoryId,
                CategoryName = c.Category.Name,
                InstructorId = c.InstructorId,
                InstructorName = c.Instructor.FullName,
                LessonCount = c.Lessons.Count,
                EnrollmentCount = c.Enrollments.Count,
                AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => (double)r.Rating) : 0,
                ReviewCount = c.Reviews.Count
            })
            .ToListAsync();

        return Result<PagedResult<CourseDto>>.Ok(PagedResult<CourseDto>.Ok(items, totalCount, page, pageSize));
    }

    public async Task<Result<CourseDto>> GetByIdAsync(int id)
    {
        var course = await courseRepository.GetAll()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
            return Result<CourseDto>.Fail("Курс не найден.", ErrorType.NotFound);

        return Result<CourseDto>.Ok(MapToDto(course));
    }

    public async Task<Result<CourseDto>> CreateAsync(CreateCourseDto dto, string instructorId)
    {
        var category = await categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category is null)
            return Result<CourseDto>.Fail("Категория не найдена.", ErrorType.NotFound);

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

        await courseRepository.AddAsync(course);
        return await GetByIdAsync(course.Id);
    }

    public async Task<Result<CourseDto>> UpdateAsync(int id, UpdateCourseDto dto, string currentUserId, bool isAdmin)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
            return Result<CourseDto>.Fail("Курс не найден.", ErrorType.NotFound);

        if (!isAdmin && course.InstructorId != currentUserId)
            return Result<CourseDto>.Fail("Нет доступа.", ErrorType.Unauthorized);

        if (dto.Title is not null) course.Title = dto.Title;
        if (dto.Description is not null) course.Description = dto.Description;
        if (dto.Price.HasValue) course.Price = dto.Price.Value;
        if (dto.Level.HasValue) course.Level = dto.Level.Value;
        if (dto.CategoryId.HasValue) course.CategoryId = dto.CategoryId.Value;

        await courseRepository.UpdateAsync(course);
        return await GetByIdAsync(course.Id);
    }

    public async Task<Result<bool>> DeleteAsync(int id, string currentUserId, bool isAdmin)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
            return Result<bool>.Fail("Курс не найден.", ErrorType.NotFound);

        if (!isAdmin && course.InstructorId != currentUserId)
            return Result<bool>.Fail("Нет доступа.", ErrorType.Unauthorized);

        await courseRepository.DeleteAsync(course);
        return Result<bool>.Ok(true);
    }

    public async Task<Result<CourseDto>> TogglePublishAsync(int id, string currentUserId)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
            return Result<CourseDto>.Fail("Курс не найден.", ErrorType.NotFound);

        if (course.InstructorId != currentUserId)
            return Result<CourseDto>.Fail("Нет доступа.", ErrorType.Unauthorized);

        course.IsPublished = !course.IsPublished;
        await courseRepository.UpdateAsync(course);
        return await GetByIdAsync(course.Id);
    }

    // public async Task<Result<string>> UploadThumbnailAsync(int id, IFormFile file, string currentUserId)
    // {
    //     var course = await courseRepository.GetByIdAsync(id);
    //     if (course is null)
    //         return Result<string>.Fail("Курс не найден.", ErrorType.NotFound);

    //     if (course.InstructorId != currentUserId)
    //         return Result<string>.Fail("Нет доступа.", ErrorType.Unauthorized);

    //     var allowedTypes = new[] { "image/jpeg", "image/png" };
    //     if (!allowedTypes.Contains(file.ContentType))
    //         return Result<string>.Fail("Разрешены только JPEG и PNG.", ErrorType.Validation);

    //     if (file.Length > 5 * 1024 * 1024)
    //         return Result<string>.Fail("Файл не должен превышать 5MB.", ErrorType.Validation);

    //     var ext = Path.GetExtension(file.FileName);
    //     var fileName = $"{id}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{ext}";
    //     var uploadPath = Path.Combine("wwwroot", "uploads", "thumbnails");
    //     Directory.CreateDirectory(uploadPath);

    //     var filePath = Path.Combine(uploadPath, fileName);
    //     await using var stream = new FileStream(filePath, FileMode.Create);
    //     await file.CopyToAsync(stream);

    //     course.ThumbnailPath = $"/uploads/thumbnails/{fileName}";
    //     await courseRepository.UpdateAsync(course);

    //     return Result<string>.Ok(course.ThumbnailPath);
    // }

    private static CourseDto MapToDto(Course c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Description = c.Description,
        ThumbnailUrl = c.ThumbnailPath,
        Price = c.Price,
        Level = c.Level,
        IsPublished = c.IsPublished,
        CreatedAt = c.CreatedAt,
        CategoryId = c.CategoryId,
        CategoryName = c.Category?.Name ?? string.Empty,
        InstructorId = c.InstructorId,
        InstructorName = c.Instructor?.FullName ?? string.Empty,
        LessonCount = c.Lessons?.Count ?? 0,
        EnrollmentCount = c.Enrollments?.Count ?? 0,
        AverageRating = c.Reviews?.Any() == true ? c.Reviews.Average(r => (double)r.Rating) : 0,
        ReviewCount = c.Reviews?.Count ?? 0
    };
}