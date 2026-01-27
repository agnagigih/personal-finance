using Finance.Api.Data;
using Finance.Api.DTOs.Category;
using Finance.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly FinanceDbContext _context;

        public CategoryService(FinanceDbContext context)
        { 
            _context = context;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(Guid userId, CreateCategoryRequest request)
        {
            var category = new Models.Category
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Type = request.Type,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
            };

            return result;
        }

        public async Task<List<CategoryResponse>> GetAllCategoryAsync(Guid userId)
        {
            var categories = await _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                }).ToListAsync();

            return categories;
        }
    }
}
