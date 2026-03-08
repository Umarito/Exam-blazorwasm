using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class CartService(IMapper mapper,ICartRepository CartRepository,ILogger<CartService> logger,IMemoryCache cache) : ICartService
{
    private readonly ICartRepository _CartRepository = CartRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<CartService> _logger = logger;
    public async Task<Response<string>> AddCartAsync(CartInsertDto CartInsertDto)
    {
        try
        {
            var Cart = _mapper.Map<Cart>(CartInsertDto);
            await _CartRepository.AddAsync(Cart);
            return new Response<string>(HttpStatusCode.OK, "Cart was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int CartId)
    {
        try
        {
            await _CartRepository.DeleteAsync(CartId);
            return new Response<string>(HttpStatusCode.OK, "Cart was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<List<CartGetDto>>> GetAllCartsAsync()
    {
        try
        {
            var messages = _CartRepository.GetAllCartsAsync();
            if(messages == null)
            {
                return new Response<List<CartGetDto>>(HttpStatusCode.NotFound,"There is no contact messages");
            }
            else
            {
                var mapped = _mapper.Map<List<CartGetDto>>(messages);
                return new Response<List<CartGetDto>>(HttpStatusCode.OK,"ok",mapped);
            }
        }
        catch(Exception ex)
        {
            return new Response<List<CartGetDto>>(HttpStatusCode.InternalServerError,ex.Message);
        }
    }

    public async Task<Response<CartGetDto>> GetCartByIdAsync(int CartId)
    {
        try
        {
            var Cart = await _CartRepository.GetByIdAsync(CartId);
            var res = _mapper.Map<CartGetDto>(Cart);
            return new Response<CartGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<CartGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int CartId, CartUpdateDto Cart)
    {
        try
        {
            var res = await _CartRepository.GetByIdAsync(CartId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Cart not found");
            }
            else
            {
                _mapper.Map(Cart, res);
                await _CartRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Cart updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}