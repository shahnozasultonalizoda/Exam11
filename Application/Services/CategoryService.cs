using Application.DTOs.Category;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;

namespace Application.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<Result<List<GetCategoryDto>>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();

        return Result<List<GetCategoryDto>>.Ok(categories.Select(c => new GetCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList());
    }

    public async Task<Result<GetCategoryDto>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return Result<GetCategoryDto>.Fail("Category not found", ErrorType.NotFound);
        }

        return Result<GetCategoryDto>.Ok(new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    public async Task<Result<GetCategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await categoryRepository.CreateAsync(category);

        return Result<GetCategoryDto>.Ok(new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return Result<bool>.Fail("Category not found", ErrorType.NotFound);
        }

        await categoryRepository.DeleteAsync(category);
        return Result<bool>.Ok(true);
    }
}