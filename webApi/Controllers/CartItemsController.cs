using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CartItemsController(ICartItemService CartItemService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddCartItemAsync(CartItemInsertDto CartItem)
    {
        return await CartItemService.AddCartItemAsync(CartItem);
    }
    [HttpPut("{CartItemId}")]
    public async Task<Response<string>> UpdateAsync(int CartItemId,CartItemUpdateDto CartItem)
    {
        return await CartItemService.UpdateAsync(CartItemId,CartItem);
    }
    [HttpDelete("{CartItemId}")]
    public async Task<Response<string>> DeleteAsync(int CartItemId)
    {
        return await CartItemService.DeleteAsync(CartItemId);
    }
    [HttpGet]
    public async Task<PagedResult<CartItemGetDto>> GetAllCartItems([FromQuery] CartItemFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await CartItemService.GetAllCartItemsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{CartItemId}")]
    public async Task<Response<CartItemGetDto>> GetCartItemByIdAsync(int CartItemId)
    {
        return await CartItemService.GetCartItemByIdAsync(CartItemId);
    }
}