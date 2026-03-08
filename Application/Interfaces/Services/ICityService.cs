using WebApi.DTOs;

public interface ICityService
{
    Task<Response<string>> AddCityAsync(CityInsertDto CityInsertDto);
    Task<Response<CityGetDto>> GetCityByIdAsync(int CityId);
    Task<PagedResult<CityGetDto>> GetAllCitiesAsync(CityFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int CityId);
    Task<Response<string>> UpdateAsync(int CityId,CityUpdateDto CityUpdateDto);
}