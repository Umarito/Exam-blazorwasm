using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class ProductImageService(IMapper mapper,IProductImageRepository ProductImageRepository,ILogger<ProductImageService> logger,IMemoryCache cache) : IProductImageService
{
    private readonly IProductImageRepository _ProductImageRepository = ProductImageRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<ProductImageService> _logger = logger;
    private string BuildCacheKey(ProductImageFilter filter, PagedQuery query)
    {
        return $"ProductImages:getAll:page={query.Page}:size={query.PageSize}:isMain={filter?.IsMain}";
    }
    public async Task<Response<string>> AddProductImageAsync(ProductImageInsertDto ProductImageInsertDto)
    {
        try
        {
            var ProductImage = _mapper.Map<ProductImage>(ProductImageInsertDto);
            await _ProductImageRepository.AddAsync(ProductImage);
            return new Response<string>(HttpStatusCode.OK, "ProductImage was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int ProductImageId)
    {
        try
        {
            await _ProductImageRepository.DeleteAsync(ProductImageId);
            return new Response<string>(HttpStatusCode.OK, "ProductImage was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<ProductImageGetDto>> GetAllProductImagesAsync(ProductImageFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<ProductImageGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var ProductImages = await _ProductImageRepository.GetAllProductImagesAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<ProductImageGetDto>>(ProductImages);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<ProductImageGetDto>> GetProductImageByIdAsync(int ProductImageId)
    {
        try
        {
            var ProductImage = await _ProductImageRepository.GetByIdAsync(ProductImageId);
            var res = _mapper.Map<ProductImageGetDto>(ProductImage);
            return new Response<ProductImageGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<ProductImageGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int ProductImageId, ProductImageUpdateDto ProductImage)
    {
        try
        {
            var res = await _ProductImageRepository.GetByIdAsync(ProductImageId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"ProductImage not found");
            }
            else
            {
                _mapper.Map(ProductImage, res);
                await _ProductImageRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"ProductImage updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}