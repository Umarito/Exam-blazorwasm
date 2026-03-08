using WebApi.DTOs;

public interface IWishlistService
{
    Task<Response<string>> AddWishlistAsync(WishlistInsertDto WishlistInsertDto);
    Task<Response<List<WishlistGetDto>>> GetAllWishlistsAsync();
    Task<Response<WishlistGetDto>> GetWishlistByIdAsync(int WishlistId);
    Task<Response<string>> DeleteAsync(int WishlistId);
    Task<Response<string>> UpdateAsync(int WishlistId,WishlistUpdateDto WishlistUpdateDto);
}