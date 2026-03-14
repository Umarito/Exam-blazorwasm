using WebApi.DTOs;

public interface IBrandRepository
{
    Task AddAsync(Brand Brand);
    Task<Brand?> GetByIdAsync(int id);
    Task<List<Brand>> GetBrandsAsync();
    Task DeleteAsync(int Brand);
    Task UpdateAsync(Brand Brand);
    Task<PagedResult<Brand>> GetAllBrandsAsync(BrandFilter filter, PagedQuery query,CancellationToken ct);
}