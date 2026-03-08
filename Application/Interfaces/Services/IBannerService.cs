using WebApi.DTOs;

public interface IBannerService
{
    Task<Response<string>> AddBannerAsync(BannerInsertDto BannerInsertDto);
    Task<Response<BannerGetDto>> GetBannerByIdAsync(int BannerId);
    Task<PagedResult<BannerGetDto>> GetAllBannersAsync(BannerFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int BannerId);
    Task<Response<string>> UpdateAsync(int BannerId,BannerUpdateDto BannerUpdateDto);
}