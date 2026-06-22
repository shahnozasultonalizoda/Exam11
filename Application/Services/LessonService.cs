using Application.DTOs.Lesson;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;

namespace Application.Services;

public class LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository) : ILessonService
{
    public async Task<Result<List<GetLessonDto>>> GetByCourseIdAsync(int courseId)
    {
        var lessons = await lessonRepository.GetByCourseIdAsync(courseId);

        return Result<List<GetLessonDto>>.Ok(lessons.Select(l => new GetLessonDto
        {
            Id = l.Id,
            Title = l.Title,
            Content = l.Content,
            Order = l.Order,
            CourseId = l.CourseId
        }).ToList());
    }

    public async Task<Result<GetLessonDto>> GetByIdAsync(int id)
    {
        var lesson = await lessonRepository.GetByIdAsync(id);
        if (lesson is null)
        {
            return Result<GetLessonDto>.Fail("Lesson not found", ErrorType.NotFound);
        }

        return Result<GetLessonDto>.Ok(new GetLessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            Order = lesson.Order,
            CourseId = lesson.CourseId
        });
    }

    public async Task<Result<GetLessonDto>> CreateAsync(CreateLessonDto dto, int instructorId)
    {
        var course = await courseRepository.GetByIdAsync(dto.CourseId);
        if (course is null)
        {
            return Result<GetLessonDto>.Fail("Course not found", ErrorType.NotFound);
        }
        if (course.InstructorId != instructorId)
        {
            return Result<GetLessonDto>.Fail("Access denied", ErrorType.Unauthorized);
        }

        var lesson = new Lesson
        {
            Title = dto.Title,
            Content = dto.Content,
            Order = dto.Order,
            CourseId = dto.CourseId,
            CreatedAt = DateTime.UtcNow
        };

        await lessonRepository.CreateAsync(lesson);

        return Result<GetLessonDto>.Ok(new GetLessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            Order = lesson.Order,
            CourseId = lesson.CourseId
        });
    }

    public async Task<Result<GetLessonDto>> UpdateAsync(int id, UpdateLessonDto dto, int instructorId)
    {
        var lesson = await lessonRepository.GetByIdAsync(id);
        if (lesson is null)
        {
            return Result<GetLessonDto>.Fail("Lesson not found", ErrorType.NotFound);
        }
        if (lesson.Course.InstructorId != instructorId)
        {
            return Result<GetLessonDto>.Fail("Access denied", ErrorType.Unauthorized);
        }

        lesson.Title = dto.Title;
        lesson.Content = dto.Content;
        lesson.Order = dto.Order;

        await lessonRepository.UpdateAsync(lesson);

        return Result<GetLessonDto>.Ok(new GetLessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            Order = lesson.Order,
            CourseId = lesson.CourseId
        });
    }

    public async Task<Result<bool>> DeleteAsync(int id, int instructorId)
    {
        var lesson = await lessonRepository.GetByIdAsync(id);
        if (lesson is null)
        {
            return Result<bool>.Fail("Lesson not found", ErrorType.NotFound);
        }
        if (lesson.Course.InstructorId != instructorId)
        {
            return Result<bool>.Fail("Access denied", ErrorType.Unauthorized);
        }

        await lessonRepository.DeleteAsync(lesson);
        return Result<bool>.Ok(true);
    }
}