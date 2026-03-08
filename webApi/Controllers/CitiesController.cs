using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CitiesController(ICityService CityService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddCityAsync(CityInsertDto City)
    {
        return await CityService.AddCityAsync(City);
    }
    [HttpPut("{CityId}")]
    public async Task<Response<string>> UpdateAsync(int CityId,CityUpdateDto City)
    {
        return await CityService.UpdateAsync(CityId,City);
    }
    [HttpDelete("{CityId}")]
    public async Task<Response<string>> DeleteAsync(int CityId)
    {
        return await CityService.DeleteAsync(CityId);
    }
    [HttpGet]
    public async Task<PagedResult<CityGetDto>> GetAllCities([FromQuery] CityFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await CityService.GetAllCitiesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{CityId}")]
    public async Task<Response<CityGetDto>> GetCityByIdAsync(int CityId)
    {
        return await CityService.GetCityByIdAsync(CityId);
    }
}