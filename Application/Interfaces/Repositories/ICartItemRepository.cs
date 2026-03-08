using WebApi.DTOs;

public interface ICartItemRepository
{
    Task AddAsync(CartItem CartItem);
    Task<CartItem?> GetByIdAsync(int id);
    Task DeleteAsync(int CartItem);
    Task UpdateAsync(CartItem CartItem);
    Task<PagedResult<CartItem>> GetAllCartItemsAsync(CartItemFilter filter, PagedQuery query,CancellationToken ct);
}