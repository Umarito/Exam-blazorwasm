using WebApi.DTOs;

public interface IProductImageService
{
    Task<Response<string>> AddProductImageAsync(ProductImageInsertDto ProductImageInsertDto);
    Task<Response<ProductImageGetDto>> GetProductImageByIdAsync(int ProductImageId);
    Task<PagedResult<ProductImageGetDto>> GetAllProductImagesAsync(ProductImageFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int ProductImageId);
    Task<Response<string>> UpdateAsync(int ProductImageId,ProductImageUpdateDto ProductImageUpdateDto);
}