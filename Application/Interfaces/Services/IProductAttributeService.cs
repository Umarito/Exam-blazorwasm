using WebApi.DTOs;

public interface IProductAttributeService
{
    Task<Response<string>> AddProductAttributeAsync(ProductAttributeInsertDto ProductAttributeInsertDto);
    Task<Response<ProductAttributeGetDto>> GetProductAttributeByIdAsync(int ProductAttributeId);
    Task<PagedResult<ProductAttributeGetDto>> GetAllProductAttributesAsync(ProductAttributeFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int ProductAttributeId);
    Task<Response<string>> UpdateAsync(int ProductAttributeId,ProductAttributeUpdateDto ProductAttributeUpdateDto);
}