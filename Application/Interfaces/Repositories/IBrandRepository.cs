using WebApi.DTOs;

public interface IBrandRepository
{
    Task AddAsync(Brand Brand);
    Task<Brand?> GetByIdAsync(int id);
    Task DeleteAsync(int Brand);
    Task UpdateAsync(Brand Brand);
    Task<PagedResult<Brand>> GetAllBrandsAsync(BrandFilter filter, PagedQuery query,CancellationToken ct);
}