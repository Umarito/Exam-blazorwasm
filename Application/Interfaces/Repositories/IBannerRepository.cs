using WebApi.DTOs;

public interface IBannerRepository
{
    Task AddAsync(Banner Banner);
    Task<Banner?> GetByIdAsync(int id);
    Task DeleteAsync(int Banner);
    Task UpdateAsync(Banner Banner);
    Task<PagedResult<Banner>> GetAllBannersAsync(BannerFilter filter, PagedQuery query,CancellationToken ct);
}