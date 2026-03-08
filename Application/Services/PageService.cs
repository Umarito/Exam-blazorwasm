using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class PageService(IMapper mapper,IPageRepository PageRepository,ILogger<PageService> logger,IMemoryCache cache) : IPageService
{
    private readonly IPageRepository _PageRepository = PageRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<PageService> _logger = logger;
    private string BuildCacheKey(PageSMSFilter filter, PagedQuery query)
    {
        return $"Pages:getAll:page={query.Page}:size={query.PageSize}:title={filter?.Title}:slug={filter?.Slug}:isPublished={filter?.IsPublished}";
    }
    public async Task<Response<string>> AddPageAsync(PageInsertDto PageInsertDto)
    {
        try
        {
            var Page = _mapper.Map<PageSMS>(PageInsertDto);
            await _PageRepository.AddAsync(Page);
            return new Response<string>(HttpStatusCode.OK, "Page was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int PageId)
    {
        try
        {
            await _PageRepository.DeleteAsync(PageId);
            return new Response<string>(HttpStatusCode.OK, "Page was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<PageGetDto>> GetAllPagesAsync(PageSMSFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<PageGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Pages = await _PageRepository.GetAllPagesAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<PageGetDto>>(Pages);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<PageGetDto>> GetPageByIdAsync(int PageId)
    {
        try
        {
            var Page = await _PageRepository.GetByIdAsync(PageId);
            var res = _mapper.Map<PageGetDto>(Page);
            return new Response<PageGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<PageGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int PageId, PageUpdateDto Page)
    {
        try
        {
            var res = await _PageRepository.GetByIdAsync(PageId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Page not found");
            }
            else
            {
                _mapper.Map(Page, res);
                await _PageRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Page updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}