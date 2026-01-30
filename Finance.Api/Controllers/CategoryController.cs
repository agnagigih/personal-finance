using Personal.Finance.Api.DTOs.Category;
using Personal.Finance.Api.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal.Finance.Api.Responses;

namespace Personal.Finance.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/category")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService servcice)
        {
            _categoryService = servcice;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategoryAsync(UserId, request);

            return Ok(ApiResponse<object>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoryAsync(UserId);

            return Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}
