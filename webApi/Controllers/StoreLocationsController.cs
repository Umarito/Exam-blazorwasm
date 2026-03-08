using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class StoreLocationsController(IStoreLocationService StoreLocationService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddStoreLocationAsync(StoreLocationInsertDto StoreLocation)
    {
        return await StoreLocationService.AddStoreLocationAsync(StoreLocation);
    }
    [HttpPut("{StoreLocationId}")]
    public async Task<Response<string>> UpdateAsync(int StoreLocationId,StoreLocationUpdateDto StoreLocation)
    {
        return await StoreLocationService.UpdateAsync(StoreLocationId,StoreLocation);
    }
    [HttpDelete("{StoreLocationId}")]
    public async Task<Response<string>> DeleteAsync(int StoreLocationId)
    {
        return await StoreLocationService.DeleteAsync(StoreLocationId);
    }
    [HttpGet]
    public async Task<PagedResult<StoreLocationGetDto>> GetAllStoreLocations([FromQuery] StoreLocationFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await StoreLocationService.GetAllStoreLocationsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{StoreLocationId}")]
    public async Task<Response<StoreLocationGetDto>> GetStoreLocationByIdAsync(int StoreLocationId)
    {
        return await StoreLocationService.GetStoreLocationByIdAsync(StoreLocationId);
    }
}