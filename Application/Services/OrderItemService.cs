using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class OrderItemService(IMapper mapper,IOrderItemRepository OrderItemRepository,ILogger<OrderItemService> logger,IMemoryCache cache) : IOrderItemService
{
    private readonly IOrderItemRepository _OrderItemRepository = OrderItemRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<OrderItemService> _logger = logger;
    private string BuildCacheKey(OrderItemFilter filter, PagedQuery query)
    {
        return $"OrderItems:getAll:page={query.Page}:size={query.PageSize}:quantity={filter?.Quantity}:unitPrice={filter?.UnitPrice}";
    }
    public async Task<Response<string>> AddOrderItemAsync(OrderItemInsertDto OrderItemInsertDto)
    {
        try
        {
            var OrderItem = _mapper.Map<OrderItem>(OrderItemInsertDto);
            await _OrderItemRepository.AddAsync(OrderItem);
            return new Response<string>(HttpStatusCode.OK, "OrderItem was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int OrderItemId)
    {
        try
        {
            await _OrderItemRepository.DeleteAsync(OrderItemId);
            return new Response<string>(HttpStatusCode.OK, "OrderItem was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<OrderItemGetDto>> GetAllOrderItemsAsync(OrderItemFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<OrderItemGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var OrderItems = await _OrderItemRepository.GetAllOrderItemsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<OrderItemGetDto>>(OrderItems);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<OrderItemGetDto>> GetOrderItemByIdAsync(int OrderItemId)
    {
        try
        {
            var OrderItem = await _OrderItemRepository.GetByIdAsync(OrderItemId);
            var res = _mapper.Map<OrderItemGetDto>(OrderItem);
            return new Response<OrderItemGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<OrderItemGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int OrderItemId, OrderItemUpdateDto OrderItem)
    {
        try
        {
            var res = await _OrderItemRepository.GetByIdAsync(OrderItemId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"OrderItem not found");
            }
            else
            {
                _mapper.Map(OrderItem, res);
                await _OrderItemRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"OrderItem updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}