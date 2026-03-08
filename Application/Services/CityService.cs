using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class CityService(IMapper mapper,ICityRepository CityRepository,ILogger<CityService> logger,IMemoryCache cache) : ICityService
{
    private readonly ICityRepository _CityRepository = CityRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<CityService> _logger = logger;
    private string BuildCacheKey(CityFilter filter, PagedQuery query)
    {
        return $"Cities:getAll:page={query.Page}:size={query.PageSize}:name={filter?.Name}";
    }
    public async Task<Response<string>> AddCityAsync(CityInsertDto CityInsertDto)
    {
        try
        {
            var City = _mapper.Map<City>(CityInsertDto);
            await _CityRepository.AddAsync(City);
            return new Response<string>(HttpStatusCode.OK, "City was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int CityId)
    {
        try
        {
            await _CityRepository.DeleteAsync(CityId);
            return new Response<string>(HttpStatusCode.OK, "City was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<CityGetDto>> GetAllCitiesAsync(CityFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<CityGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Cities = await _CityRepository.GetAllCitiesAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<CityGetDto>>(Cities);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<CityGetDto>> GetCityByIdAsync(int CityId)
    {
        try
        {
            var City = await _CityRepository.GetByIdAsync(CityId);
            var res = _mapper.Map<CityGetDto>(City);
            return new Response<CityGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<CityGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int CityId, CityUpdateDto City)
    {
        try
        {
            var res = await _CityRepository.GetByIdAsync(CityId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"City not found");
            }
            else
            {
                _mapper.Map(City, res);
                await _CityRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"City updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}