using WebApi.DTOs;

public interface IStoreLocationRepository
{
    Task AddAsync(StoreLocation ApplicationStoreLocation);
    Task<StoreLocation?> GetByIdAsync(int id);
    Task DeleteAsync(int ApplicationStoreLocation);
    Task UpdateAsync(StoreLocation ApplicationStoreLocation);
    Task<PagedResult<StoreLocation>> GetAllStoreLocationsAsync(StoreLocationFilter filter, PagedQuery query,CancellationToken ct);
}