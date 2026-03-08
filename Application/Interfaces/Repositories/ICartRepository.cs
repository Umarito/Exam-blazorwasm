using WebApi.DTOs;

public interface ICartRepository
{
    Task AddAsync(Cart Cart);
    Task<Cart?> GetByIdAsync(int id);
    Task DeleteAsync(int Cart);
    Task UpdateAsync(Cart Cart);
    Task<List<Cart>> GetAllCartsAsync();
}