using WebApi.DTOs;

public interface IWishlistRepository
{
    Task AddAsync(Wishlist Wishlist);
    Task<Wishlist?> GetByIdAsync(int id);
    Task DeleteAsync(int Wishlist);
    Task UpdateAsync(Wishlist Wishlist);
    Task<List<Wishlist>> GetAllWishlistsAsync();
}