using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class ProductAttributeService(IMapper mapper,IProductAttributeRepository ProductAttributeRepository,ILogger<ProductAttributeService> logger,IMemoryCache cache) : IProductAttributeService
{
    private readonly IProductAttributeRepository _ProductAttributeRepository = ProductAttributeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<ProductAttributeService> _logger = logger;
    private string BuildCacheKey(ProductAttributeFilter filter, PagedQuery query)
    {
        return $"ProductAttributes:getAll:page={query.Page}:size={query.PageSize}:value={filter?.Value}";
    }
    public async Task<Response<string>> AddProductAttributeAsync(ProductAttributeInsertDto ProductAttributeInsertDto)
    {
        try
        {
            var ProductAttribute = _mapper.Map<ProductAttribute>(ProductAttributeInsertDto);
            await _ProductAttributeRepository.AddAsync(ProductAttribute);
            return new Response<string>(HttpStatusCode.OK, "ProductAttribute was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int ProductAttributeId)
    {
        try
        {
            await _ProductAttributeRepository.DeleteAsync(ProductAttributeId);
            return new Response<string>(HttpStatusCode.OK, "ProductAttribute was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<ProductAttributeGetDto>> GetAllProductAttributesAsync(ProductAttributeFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<ProductAttributeGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var ProductAttributes = await _ProductAttributeRepository.GetAllProductAttributesAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<ProductAttributeGetDto>>(ProductAttributes);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<ProductAttributeGetDto>> GetProductAttributeByIdAsync(int ProductAttributeId)
    {
        try
        {
            var ProductAttribute = await _ProductAttributeRepository.GetByIdAsync(ProductAttributeId);
            var res = _mapper.Map<ProductAttributeGetDto>(ProductAttribute);
            return new Response<ProductAttributeGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<ProductAttributeGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int ProductAttributeId, ProductAttributeUpdateDto ProductAttribute)
    {
        try
        {
            var res = await _ProductAttributeRepository.GetByIdAsync(ProductAttributeId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"ProductAttribute not found");
            }
            else
            {
                _mapper.Map(ProductAttribute, res);
                await _ProductAttributeRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"ProductAttribute updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}