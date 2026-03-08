using WebApi.DTOs;

public interface IBrandService
{
    Task<Response<string>> AddBrandAsync(BrandInsertDto BrandInsertDto);
    Task<Response<BrandGetDto>> GetBrandByIdAsync(int BrandId);
    Task<PagedResult<BrandGetDto>> GetAllBrandsAsync(BrandFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int BrandId);
    Task<Response<string>> UpdateAsync(int BrandId,BrandUpdateDto BrandUpdateDto);
}