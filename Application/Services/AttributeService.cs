using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class AttributeService(IMapper mapper,IAttributeRepository AttributeRepository,ILogger<AttributeService> logger,IMemoryCache cache) : IAttributeService
{
    private readonly IAttributeRepository _AttributeRepository = AttributeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<AttributeService> _logger = logger;
    private string BuildCacheKey(AttributeFilter filter, PagedQuery query)
    {
        return $"Attributes:getAll:page={query.Page}:size={query.PageSize}:name={filter?.Name}";
    }
    public async Task<Response<string>> AddAttributeAsync(AttributeInsertDto AttributeInsertDto)
    {
        try
        {
            var Attribute = _mapper.Map<Attribute>(AttributeInsertDto);
            await _AttributeRepository.AddAsync(Attribute);
            return new Response<string>(HttpStatusCode.OK, "Attribute was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int AttributeId)
    {
        try
        {
            await _AttributeRepository.DeleteAsync(AttributeId);
            return new Response<string>(HttpStatusCode.OK, "Attribute was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<AttributeGetDto>> GetAllAttributesAsync(AttributeFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<AttributeGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Attributes = await _AttributeRepository.GetAllAttributesAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<AttributeGetDto>>(Attributes);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<AttributeGetDto>> GetAttributeByIdAsync(int AttributeId)
    {
        try
        {
            var Attribute = await _AttributeRepository.GetByIdAsync(AttributeId);
            var res = _mapper.Map<AttributeGetDto>(Attribute);
            return new Response<AttributeGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<AttributeGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int AttributeId, AttributeUpdateDto Attribute)
    {
        try
        {
            var res = await _AttributeRepository.GetByIdAsync(AttributeId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Attribute not found");
            }
            else
            {
                _mapper.Map(Attribute, res);
                await _AttributeRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Attribute updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}