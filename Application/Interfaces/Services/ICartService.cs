using WebApi.DTOs;

public interface ICartService
{
    Task<Response<string>> AddCartAsync(CartInsertDto CartInsertDto);
    Task<Response<List<CartGetDto>>> GetAllCartsAsync();
    Task<Response<CartGetDto>> GetCartByIdAsync(int CartId);
    Task<Response<string>> DeleteAsync(int CartId);
    Task<Response<string>> UpdateAsync(int CartId,CartUpdateDto CartUpdateDto);
}