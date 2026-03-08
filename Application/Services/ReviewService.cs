using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class ReviewService(IMapper mapper,IReviewRepository ReviewRepository,ILogger<ReviewService> logger,IMemoryCache cache) : IReviewService
{
    private readonly IReviewRepository _ReviewRepository = ReviewRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<ReviewService> _logger = logger;
    private string BuildCacheKey(ReviewFilter filter, PagedQuery query)
    {
        return $"Reviews:getAll:page={query.Page}:size={query.PageSize}:isApproved={filter?.IsApproved}:rating={filter?.Rating}";
    }
    public async Task<Response<string>> AddReviewAsync(ReviewInsertDto ReviewInsertDto)
    {
        try
        {
            var Review = _mapper.Map<Review>(ReviewInsertDto);
            await _ReviewRepository.AddAsync(Review);
            return new Response<string>(HttpStatusCode.OK, "Review was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int ReviewId)
    {
        try
        {
            await _ReviewRepository.DeleteAsync(ReviewId);
            return new Response<string>(HttpStatusCode.OK, "Review was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<ReviewGetDto>> GetAllReviewsAsync(ReviewFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<ReviewGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Reviews = await _ReviewRepository.GetAllReviewsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<ReviewGetDto>>(Reviews);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<ReviewGetDto>> GetReviewByIdAsync(int ReviewId)
    {
        try
        {
            var Review = await _ReviewRepository.GetByIdAsync(ReviewId);
            var res = _mapper.Map<ReviewGetDto>(Review);
            return new Response<ReviewGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<ReviewGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int ReviewId, ReviewUpdateDto Review)
    {
        try
        {
            var res = await _ReviewRepository.GetByIdAsync(ReviewId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Review not found");
            }
            else
            {
                _mapper.Map(Review, res);
                await _ReviewRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Review updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}