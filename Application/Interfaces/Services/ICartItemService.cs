using WebApi.DTOs;

public interface ICartItemService
{
    Task<Response<string>> AddCartItemAsync(CartItemInsertDto CartItemInsertDto);
    Task<Response<CartItemGetDto>> GetCartItemByIdAsync(int CartItemId);
    Task<PagedResult<CartItemGetDto>> GetAllCartItemsAsync(CartItemFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int CartItemId);
    Task<Response<string>> UpdateAsync(int CartItemId,CartItemUpdateDto CartItemUpdateDto);
}