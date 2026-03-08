using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class BannerService(IMapper mapper,IBannerRepository BannerRepository,ILogger<BannerService> logger,IMemoryCache cache) : IBannerService
{
    private readonly IBannerRepository _BannerRepository = BannerRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<BannerService> _logger = logger;
    private string BuildCacheKey(BannerFilter filter, PagedQuery query)
    {
        return $"Banners:getAll:page={query.Page}:size={query.PageSize}:imageUrl={filter?.ImageUrl}";
    }
    public async Task<Response<string>> AddBannerAsync(BannerInsertDto BannerInsertDto)
    {
        try
        {
            var Banner = _mapper.Map<Banner>(BannerInsertDto);
            await _BannerRepository.AddAsync(Banner);
            return new Response<string>(HttpStatusCode.OK, "Banner was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int BannerId)
    {
        try
        {
            await _BannerRepository.DeleteAsync(BannerId);
            return new Response<string>(HttpStatusCode.OK, "Banner was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<BannerGetDto>> GetAllBannersAsync(BannerFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<BannerGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Banners = await _BannerRepository.GetAllBannersAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<BannerGetDto>>(Banners);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<BannerGetDto>> GetBannerByIdAsync(int BannerId)
    {
        try
        {
            var Banner = await _BannerRepository.GetByIdAsync(BannerId);
            var res = _mapper.Map<BannerGetDto>(Banner);
            return new Response<BannerGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<BannerGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int BannerId, BannerUpdateDto Banner)
    {
        try
        {
            var res = await _BannerRepository.GetByIdAsync(BannerId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Banner not found");
            }
            else
            {
                _mapper.Map(Banner, res);
                await _BannerRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Banner updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}