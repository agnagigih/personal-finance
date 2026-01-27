using Finance.Api.DTOs.Category;

namespace Finance.Api.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategoryAsync(Guid userId, CreateCategoryRequest request);

        Task<List<CategoryResponse>> GetAllCategoryAsync (Guid userId);
    }
}
