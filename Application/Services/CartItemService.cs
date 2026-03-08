using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class CartItemService(IMapper mapper,ICartItemRepository CartItemRepository,ILogger<CartItemService> logger,IMemoryCache cache) : ICartItemService
{
    private readonly ICartItemRepository _CartItemRepository = CartItemRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<CartItemService> _logger = logger;
    private string BuildCacheKey(CartItemFilter filter, PagedQuery query)
    {
        return $"CartItems:getAll:page={query.Page}:size={query.PageSize}:unitPrice={filter?.UnitPrice}:quantity={filter?.Quantity}";
    }
    public async Task<Response<string>> AddCartItemAsync(CartItemInsertDto CartItemInsertDto)
    {
        try
        {
            var CartItem = _mapper.Map<CartItem>(CartItemInsertDto);
            await _CartItemRepository.AddAsync(CartItem);
            return new Response<string>(HttpStatusCode.OK, "CartItem was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int CartItemId)
    {
        try
        {
            await _CartItemRepository.DeleteAsync(CartItemId);
            return new Response<string>(HttpStatusCode.OK, "CartItem was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<CartItemGetDto>> GetAllCartItemsAsync(CartItemFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<CartItemGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var CartItems = await _CartItemRepository.GetAllCartItemsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<CartItemGetDto>>(CartItems);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<CartItemGetDto>> GetCartItemByIdAsync(int CartItemId)
    {
        try
        {
            var CartItem = await _CartItemRepository.GetByIdAsync(CartItemId);
            var res = _mapper.Map<CartItemGetDto>(CartItem);
            return new Response<CartItemGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<CartItemGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int CartItemId, CartItemUpdateDto CartItem)
    {
        try
        {
            var res = await _CartItemRepository.GetByIdAsync(CartItemId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"CartItem not found");
            }
            else
            {
                _mapper.Map(CartItem, res);
                await _CartItemRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"CartItem updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}