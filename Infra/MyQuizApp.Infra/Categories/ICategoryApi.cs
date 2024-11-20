using MyQuizApp.WebApi.Services;
using Refit;

namespace MyQuizApp.Infra.Categories;

public interface ICategoryApi
{
    [Get("/api/categories")]
    Task<ApiResponseWithData<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();

    [Get("/api/categories/{id}")]
    Task<ApiResponseWithData<CategoryDto>> GetCategoryByIdAsync(Guid id);

    [Post("/api/categories")]
    Task<ApiResponseWithData<CategoryDto>> CreateCategoryAsync([Body] CreateCategoryDto createDto);

    [Put("/api/categories/{id}")]
    Task<ApiResponseWithData<CategoryDto>> UpdateCategoryAsync(Guid id, [Body] UpdateCategoryDto updateDto);

    [Delete("/api/categories/{id}")]
    Task<ApiResponse> DeleteCategoryAsync(Guid id);
}