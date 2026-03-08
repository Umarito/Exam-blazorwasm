using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class WishlistService(IMapper mapper,IWishlistRepository WishlistRepository,ILogger<WishlistService> logger,IMemoryCache cache) : IWishlistService
{
    private readonly IWishlistRepository _WishlistRepository = WishlistRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<WishlistService> _logger = logger;
    public async Task<Response<string>> AddWishlistAsync(WishlistInsertDto WishlistInsertDto)
    {
        try
        {
            var Wishlist = _mapper.Map<Wishlist>(WishlistInsertDto);
            await _WishlistRepository.AddAsync(Wishlist);
            return new Response<string>(HttpStatusCode.OK, "Wishlist was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int WishlistId)
    {
        try
        {
            await _WishlistRepository.DeleteAsync(WishlistId);
            return new Response<string>(HttpStatusCode.OK, "Wishlist was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<List<WishlistGetDto>>> GetAllWishlistsAsync()
    {
        try
        {
            var messages = _WishlistRepository.GetAllWishlistsAsync();
            if(messages == null)
            {
                return new Response<List<WishlistGetDto>>(HttpStatusCode.NotFound,"There is no contact messages");
            }
            else
            {
                var mapped = _mapper.Map<List<WishlistGetDto>>(messages);
                return new Response<List<WishlistGetDto>>(HttpStatusCode.OK,"ok",mapped);
            }
        }
        catch(Exception ex)
        {
            return new Response<List<WishlistGetDto>>(HttpStatusCode.InternalServerError,ex.Message);
        }
    }

    public async Task<Response<WishlistGetDto>> GetWishlistByIdAsync(int WishlistId)
    {
        try
        {
            var Wishlist = await _WishlistRepository.GetByIdAsync(WishlistId);
            var res = _mapper.Map<WishlistGetDto>(Wishlist);
            return new Response<WishlistGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<WishlistGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int WishlistId, WishlistUpdateDto Wishlist)
    {
        try
        {
            var res = await _WishlistRepository.GetByIdAsync(WishlistId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Wishlist not found");
            }
            else
            {
                _mapper.Map(Wishlist, res);
                await _WishlistRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Wishlist updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}