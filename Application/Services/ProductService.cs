using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class ProductService(IMapper mapper,IProductRepository ProductRepository,ILogger<ProductService> logger,IMemoryCache cache) : IProductService
{
    private readonly IProductRepository _ProductRepository = ProductRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<ProductService> _logger = logger;
    private string BuildCacheKey(ProductFilter filter, PagedQuery query)
    {
        return $"Products:getAll:page={query.Page}:size={query.PageSize}:name={filter?.Name}:price={filter?.Price}:isActive={filter?.IsActive}";
    }
    public async Task<Response<string>> AddProductAsync(ProductInsertDto ProductInsertDto)
    {
        try
        {
            var Product = _mapper.Map<Product>(ProductInsertDto);
            await _ProductRepository.AddAsync(Product);
            return new Response<string>(HttpStatusCode.OK, "Product was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int ProductId)
    {
        try
        {
            await _ProductRepository.DeleteAsync(ProductId);
            return new Response<string>(HttpStatusCode.OK, "Product was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<ProductGetDto>> GetAllProductsAsync(ProductFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<ProductGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Products = await _ProductRepository.GetAllProductsAsync(filter, query, ct);

        var res = new PagedResult<ProductGetDto>
        {
            Items = _mapper.Map<List<ProductGetDto>>(Products.Items),
            Page = Products.Page,
            PageSize = Products.PageSize,
            TotalCount = Products.TotalCount,
            TotalPages = Products.TotalPages
        };

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<ProductGetDto>> GetProductByIdAsync(int ProductId)
    {
        try
        {
            var Product = await _ProductRepository.GetByIdAsync(ProductId);
            var res = _mapper.Map<ProductGetDto>(Product);
            return new Response<ProductGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<ProductGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int ProductId, ProductUpdateDto Product)
    {
        try
        {
            var res = await _ProductRepository.GetByIdAsync(ProductId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Product not found");
            }
            else
            {
                _mapper.Map(Product, res);
                await _ProductRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Product updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}