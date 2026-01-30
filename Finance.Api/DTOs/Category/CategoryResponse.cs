using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.DTOs.Category
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public CategoryType Type { get; set; }
    }
}
