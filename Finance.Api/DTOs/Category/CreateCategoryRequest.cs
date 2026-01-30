using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.DTOs.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public CategoryType Type { get; set; }
    }
}
