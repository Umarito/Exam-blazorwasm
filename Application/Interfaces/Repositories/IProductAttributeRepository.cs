using WebApi.DTOs;

public interface IProductAttributeRepository
{
    Task AddAsync(ProductAttribute ProductAttribute);
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task DeleteAsync(int ProductAttribute);
    Task UpdateAsync(ProductAttribute ProductAttribute);
    Task<PagedResult<ProductAttribute>> GetAllProductAttributesAsync(ProductAttributeFilter filter, PagedQuery query,CancellationToken ct);
}