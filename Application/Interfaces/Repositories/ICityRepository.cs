using WebApi.DTOs;

public interface ICityRepository
{
    Task AddAsync(City City);
    Task<City?> GetByIdAsync(int id);
    Task DeleteAsync(int City);
    Task UpdateAsync(City City);
    Task<PagedResult<City>> GetAllCitiesAsync(CityFilter filter, PagedQuery query,CancellationToken ct);
}