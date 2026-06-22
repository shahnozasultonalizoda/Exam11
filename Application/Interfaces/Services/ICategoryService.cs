using Application.DTOs.Category;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<List<GetCategoryDto>>> GetAllAsync();
    Task<Result<GetCategoryDto>> GetByIdAsync(int id);
    Task<Result<GetCategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
