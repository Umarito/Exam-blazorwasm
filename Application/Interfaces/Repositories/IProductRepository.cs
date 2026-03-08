using WebApi.DTOs;

public interface IProductRepository
{
    Task AddAsync(Product Product);
    Task<Product?> GetByIdAsync(int id);
    Task DeleteAsync(int Product);
    Task UpdateAsync(Product Product);
    Task<PagedResult<Product>> GetAllProductsAsync(ProductFilter filter, PagedQuery query,CancellationToken ct);
}