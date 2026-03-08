using WebApi.DTOs;

public interface IProductService
{
    Task<Response<string>> AddProductAsync(ProductInsertDto ProductInsertDto);
    Task<Response<ProductGetDto>> GetProductByIdAsync(int ProductId);
    Task<PagedResult<ProductGetDto>> GetAllProductsAsync(ProductFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int ProductId);
    Task<Response<string>> UpdateAsync(int ProductId,ProductUpdateDto ProductUpdateDto);
}