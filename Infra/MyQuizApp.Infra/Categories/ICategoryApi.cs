using MyQuizApp.WebApi.Services;
using Refit;

namespace MyQuizApp.Infra.Categories;

[Headers("authorization: Bearer ")]
public interface ICategoryApi
{
    [Get("/api/categories")]
    Task<ApiResponseWithData<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();

    [Get("/api/categories/{id}")]
    Task<ApiResponseWithData<CategoryDto>> GetCategoryByIdAsync(Guid id);

    [Post("/api/categories")]
    Task<ApiResponseWithData<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto);

    [Put("/api/categories/{id}")]
    Task<ApiResponseWithData<CategoryDto>> UpdateCategoryAsync(Guid id,UpdateCategoryDto updateDto);

    [Delete("/api/categories/{id}")]
    Task<ApiResponse> DeleteCategoryAsync(Guid id);
}