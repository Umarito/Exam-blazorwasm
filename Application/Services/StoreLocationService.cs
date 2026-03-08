using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class StoreLocationService(IMapper mapper,IStoreLocationRepository StoreLocationRepository,ILogger<StoreLocationService> logger,IMemoryCache cache) : IStoreLocationService
{
    private readonly IStoreLocationRepository _StoreLocationRepository = StoreLocationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<StoreLocationService> _logger = logger;
    private string BuildCacheKey(StoreLocationFilter filter, PagedQuery query)
    {
        return $"StoreLocations:getAll:page={query.Page}:size={query.PageSize}:address={filter?.Address}:mapCoordinates={filter?.MapCoordinates}:workingHours={filter?.WorkingHours}";
    }
    public async Task<Response<string>> AddStoreLocationAsync(StoreLocationInsertDto StoreLocationInsertDto)
    {
        try
        {
            var StoreLocation = _mapper.Map<StoreLocation>(StoreLocationInsertDto);
            await _StoreLocationRepository.AddAsync(StoreLocation);
            return new Response<string>(HttpStatusCode.OK, "StoreLocation was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int StoreLocationId)
    {
        try
        {
            await _StoreLocationRepository.DeleteAsync(StoreLocationId);
            return new Response<string>(HttpStatusCode.OK, "StoreLocation was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<StoreLocationGetDto>> GetAllStoreLocationsAsync(StoreLocationFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<StoreLocationGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var StoreLocations = await _StoreLocationRepository.GetAllStoreLocationsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<StoreLocationGetDto>>(StoreLocations);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<StoreLocationGetDto>> GetStoreLocationByIdAsync(int StoreLocationId)
    {
        try
        {
            var StoreLocation = await _StoreLocationRepository.GetByIdAsync(StoreLocationId);
            var res = _mapper.Map<StoreLocationGetDto>(StoreLocation);
            return new Response<StoreLocationGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<StoreLocationGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int StoreLocationId, StoreLocationUpdateDto StoreLocation)
    {
        try
        {
            var res = await _StoreLocationRepository.GetByIdAsync(StoreLocationId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"StoreLocation not found");
            }
            else
            {
                _mapper.Map(StoreLocation, res);
                await _StoreLocationRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"StoreLocation updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}