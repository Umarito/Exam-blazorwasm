using WebApi.DTOs;

public interface IProductImageRepository
{
    Task AddAsync(ProductImage ProductImage);
    Task<ProductImage?> GetByIdAsync(int id);
    Task DeleteAsync(int ProductImage);
    Task UpdateAsync(ProductImage ProductImage);
    Task<PagedResult<ProductImage>> GetAllProductImagesAsync(ProductImageFilter filter, PagedQuery query,CancellationToken ct);
}