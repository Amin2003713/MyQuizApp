using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using MyQuizApp.Domain.Categories;
using MyQuizApp.Infra.Services;
using MyQuizApp.WebApi.Data;
using Refit;

namespace MyQuizApp.WebApi.Services;

public class CategoryService(Context dbContext)
{
   public async Task<ApiResponseWithData<IEnumerable<CategoryDto>>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories.AsNoTracking()
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        return ApiResponseWithData<IEnumerable<CategoryDto>>.Success(categories);
    }

    public async Task<ApiResponseWithData<CategoryDto>> GetCategoryByIdAsync(Guid id)
    {
        var category = await dbContext.Categories.AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto(c.Id, c.Name))
            .FirstOrDefaultAsync();

        if (category == null)
            return ApiResponseWithData<CategoryDto>.Failed(null, "Category not found");

        return ApiResponseWithData<CategoryDto>.Success(category);
    }

    public async Task<ApiResponseWithData<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto)
    {
        if (dbContext.Categories.Any(c => c.Name == createDto.Name))
            return ApiResponseWithData<CategoryDto>.Failed(null!, "A category with the same name already exists.");

        var category = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = createDto.Name
        };

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var categoryDto = new CategoryDto(category.Id, category.Name);
        return ApiResponseWithData<CategoryDto>.Success(categoryDto);
    }

    public async Task<ApiResponseWithData<CategoryDto>> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateDto)
    {
        var existingCategory = await dbContext.Categories.FindAsync(id);

        if (existingCategory == null)
            return ApiResponseWithData<CategoryDto>.Failed(null, "Category not found");

        existingCategory.Name = updateDto.Name;
        await dbContext.SaveChangesAsync();

        var updatedCategoryDto = new CategoryDto(existingCategory.Id, existingCategory.Name);
        return ApiResponseWithData<CategoryDto>.Success(updatedCategoryDto);
    }

    public async Task<ApiResponse> DeleteCategoryAsync(Guid id)
    {
        var category = await dbContext.Categories.FindAsync(id);

        if (category == null)
            return ApiResponse.Failed("Category not found");

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return ApiResponse.Success("Category deleted successfully");
    }
}