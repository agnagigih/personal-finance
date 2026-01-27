using Finance.Api.Models;

namespace Finance.Api.DTOs.Category
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public CategoryType Type { get; set; }
    }
}
