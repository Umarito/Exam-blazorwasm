using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CartsController(ICartService CartService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddCartAsync(CartInsertDto Cart)
    {
        return await CartService.AddCartAsync(Cart);
    }
    [HttpPut("{CartId}")]
    public async Task<Response<string>> UpdateAsync(int CartId,CartUpdateDto Cart)
    {
        return await CartService.UpdateAsync(CartId,Cart);
    }
    [HttpDelete("{CartId}")]
    public async Task<Response<string>> DeleteAsync(int CartId)
    {
        return await CartService.DeleteAsync(CartId);
    }
    [HttpGet]
    public async Task<Response<List<CartGetDto>>> GetAllCarts()
    {
        return await CartService.GetAllCartsAsync();   
    }
    
    [HttpGet("{CartId}")]
    public async Task<Response<CartGetDto>> GetCartByIdAsync(int CartId)
    {
        return await CartService.GetCartByIdAsync(CartId);
    }
}