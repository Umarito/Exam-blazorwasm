using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService CategoryService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddCategoryAsync(CategoryInsertDto Category)
    {
        return await CategoryService.AddCategoryAsync(Category);
    }
    [HttpPut("{CategoryId}")]
    public async Task<Response<string>> UpdateAsync(int CategoryId,CategoryUpdateDto Category)
    {
        return await CategoryService.UpdateAsync(CategoryId,Category);
    }
    [HttpDelete("{CategoryId}")]
    public async Task<Response<string>> DeleteAsync(int CategoryId)
    {
        return await CategoryService.DeleteAsync(CategoryId);
    }
    [HttpGet]
    public async Task<PagedResult<CategoryGetDto>> GetAllCategories([FromQuery] CategoryFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await CategoryService.GetAllCategoriesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{CategoryId}")]
    public async Task<Response<CategoryGetDto>> GetCategoryByIdAsync(int CategoryId)
    {
        return await CategoryService.GetCategoryByIdAsync(CategoryId);
    }
}