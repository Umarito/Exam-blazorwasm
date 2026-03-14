using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class BrandService(IMapper mapper,IBrandRepository BrandRepository,ILogger<BrandService> logger,IMemoryCache cache) : IBrandService
{
    private readonly IBrandRepository _BrandRepository = BrandRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<BrandService> _logger = logger;
    private string BuildCacheKey(BrandFilter filter, PagedQuery query)
    {
        return $"Brands:getAll:page={query.Page}:size={query.PageSize}:name={filter?.Name}:slug={filter?.Slug}";
    }
    public async Task<Response<string>> AddBrandAsync(BrandInsertDto BrandInsertDto)
    {
        try
        {
            var Brand = _mapper.Map<Brand>(BrandInsertDto);
            await _BrandRepository.AddAsync(Brand);
            return new Response<string>(HttpStatusCode.OK, "Brand was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int BrandId)
    {
        try
        {
            await _BrandRepository.DeleteAsync(BrandId);
            return new Response<string>(HttpStatusCode.OK, "Brand was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<BrandGetDto>> GetAllBrandsAsync(BrandFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<BrandGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Brands = await _BrandRepository.GetAllBrandsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<BrandGetDto>>(Brands);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<BrandGetDto>> GetBrandByIdAsync(int BrandId)
    {
        try
        {
            var Brand = await _BrandRepository.GetByIdAsync(BrandId);
            var res = _mapper.Map<BrandGetDto>(Brand);
            return new Response<BrandGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<BrandGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int BrandId, BrandUpdateDto Brand)
    {
        try
        {
            var res = await _BrandRepository.GetByIdAsync(BrandId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Brand not found");
            }
            else
            {
                _mapper.Map(Brand, res);
                await _BrandRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Brand updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
    // public async Task<List<BrandGetDto>> GetAllBrandAsync()
    // {
    //     try
    //     {
    //         var messages = _BrandRepository.GetBrandsAsync();
    //         if(messages == null)
    //         {
    //             return new <List<BrandGetDto>>;
    //         }
    //         else
    //         {
    //             var mapped = _mapper.Map<List<BrandGetDto>>(messages);
    //             return new Response<List<BrandGetDto>>(HttpStatusCode.OK,"ok",mapped);
    //         }
    //     }
    //     catch(Exception ex)
    //     {
    //         return new Response<List<BrandGetDto>>(HttpStatusCode.InternalServerError,ex.Message);
    //     }
    // }
}