using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class WishlistsController(IWishlistService WishlistService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddWishlistAsync(WishlistInsertDto Wishlist)
    {
        return await WishlistService.AddWishlistAsync(Wishlist);
    }
    [HttpPut("{WishlistId}")]
    public async Task<Response<string>> UpdateAsync(int WishlistId,WishlistUpdateDto Wishlist)
    {
        return await WishlistService.UpdateAsync(WishlistId,Wishlist);
    }
    [HttpDelete("{WishlistId}")]
    public async Task<Response<string>> DeleteAsync(int WishlistId)
    {
        return await WishlistService.DeleteAsync(WishlistId);
    }
    [HttpGet]
    public async Task<Response<List<WishlistGetDto>>> GetAllWishlists()
    {
        return await WishlistService.GetAllWishlistsAsync();   
    }
    
    [HttpGet("{WishlistId}")]
    public async Task<Response<WishlistGetDto>> GetWishlistByIdAsync(int WishlistId)
    {
        return await WishlistService.GetWishlistByIdAsync(WishlistId);
    }
}