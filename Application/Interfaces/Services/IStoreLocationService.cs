using WebApi.DTOs;

public interface IStoreLocationService
{
    Task<Response<string>> AddStoreLocationAsync(StoreLocationInsertDto StoreLocationInsertDto);
    Task<Response<StoreLocationGetDto>> GetStoreLocationByIdAsync(int StoreLocationId);
    Task<PagedResult<StoreLocationGetDto>> GetAllStoreLocationsAsync(StoreLocationFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int StoreLocationId);
    Task<Response<string>> UpdateAsync(int StoreLocationId,StoreLocationUpdateDto StoreLocationUpdateDto);
}