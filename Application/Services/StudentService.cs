using Application.DTOs.Student;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;

namespace Application.Services;

public class StudentService(IStudentRepository studentRepository) : IStudentService
{
    public async Task<Result<List<GetStudentDto>>> GetAllAsync()
    {
        var students = await studentRepository.GetAllAsync();

        return Result<List<GetStudentDto>>.Ok(students.Select(s => new GetStudentDto
        {
            Id = s.Id,
            FullName = s.User.FullName,
            Email = s.User.Email,
            Bio = s.Bio
        }).ToList());
    }

    public async Task<Result<GetStudentDto>> GetByIdAsync(int id)
    {
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return Result<GetStudentDto>.Fail("Student not found", ErrorType.NotFound);
        }

        return Result<GetStudentDto>.Ok(new GetStudentDto
        {
            Id = student.Id,
            FullName = student.User.FullName,
            Email = student.User.Email,
            Bio = student.Bio
        });
    }

    public async Task<Result<GetStudentDto>> UpdateAsync(int id, UpdateStudentDto dto, int currentUserId)
    {
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return Result<GetStudentDto>.Fail("Student not found", ErrorType.NotFound);
        }
        if (student.UserId != currentUserId)
        {
            return Result<GetStudentDto>.Fail("Access denied", ErrorType.Unauthorized);
        }

        student.Bio = dto.Bio;
        student.User.FullName = dto.FullName;

        await studentRepository.UpdateAsync(student);

        return Result<GetStudentDto>.Ok(new GetStudentDto
        {
            Id = student.Id,
            FullName = student.User.FullName,
            Email = student.User.Email,
            Bio = student.Bio
        });
    }
}