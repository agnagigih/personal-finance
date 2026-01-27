using Finance.Api.Models;

namespace Finance.Api.DTOs.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public CategoryType Type { get; set; }
    }
}
