using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class OrderService(IMapper mapper,IOrderRepository OrderRepository,ILogger<OrderService> logger,IMemoryCache cache) : IOrderService
{
    private readonly IOrderRepository _OrderRepository = OrderRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<OrderService> _logger = logger;
    private string BuildCacheKey(OrderFilter filter, PagedQuery query)
    {
        return $"Orders:getAll:page={query.Page}:size={query.PageSize}:deliveryAddress={filter?.DeliveryAddress}:phone={filter?.Phone}:status={filter?.Status}:totalAmount={filter?.TotalAmount}";
    }
    public async Task<Response<string>> AddOrderAsync(OrderInsertDto OrderInsertDto)
    {
        try
        {
            var Order = _mapper.Map<Order>(OrderInsertDto);
            await _OrderRepository.AddAsync(Order);
            return new Response<string>(HttpStatusCode.OK, "Order was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int OrderId)
    {
        try
        {
            await _OrderRepository.DeleteAsync(OrderId);
            return new Response<string>(HttpStatusCode.OK, "Order was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<OrderGetDto>> GetAllOrdersAsync(OrderFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<OrderGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Orders = await _OrderRepository.GetAllOrdersAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<OrderGetDto>>(Orders);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<OrderGetDto>> GetOrderByIdAsync(int OrderId)
    {
        try
        {
            var Order = await _OrderRepository.GetByIdAsync(OrderId);
            var res = _mapper.Map<OrderGetDto>(Order);
            return new Response<OrderGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<OrderGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int OrderId, OrderUpdateDto Order)
    {
        try
        {
            var res = await _OrderRepository.GetByIdAsync(OrderId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Order not found");
            }
            else
            {
                _mapper.Map(Order, res);
                await _OrderRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Order updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}