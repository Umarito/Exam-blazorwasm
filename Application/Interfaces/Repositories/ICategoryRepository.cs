using WebApi.DTOs;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(int id);
    Task DeleteAsync(int category);
    Task UpdateAsync(Category category);
    Task<PagedResult<Category>> GetAllCategoriesAsync(CategoryFilter filter, PagedQuery query,CancellationToken ct);
}