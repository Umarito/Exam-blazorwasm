using WebApi.DTOs;

public interface ICategoryService
{
    Task<Response<string>> AddCategoryAsync(CategoryInsertDto CategoryInsertDto);
    Task<Response<CategoryGetDto>> GetCategoryByIdAsync(int CategoryId);
    Task<PagedResult<CategoryGetDto>> GetAllCategoriesAsync(CategoryFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int CategoryId);
    Task<Response<string>> UpdateAsync(int CategoryId,CategoryUpdateDto CategoryUpdateDto);
}