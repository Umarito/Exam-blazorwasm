using WebApi.DTOs;

public interface IOrderRepository
{
    Task AddAsync(Order Order);
    Task<Order?> GetByIdAsync(int id);
    Task DeleteAsync(int Order);
    Task UpdateAsync(Order Order);
    Task<PagedResult<Order>> GetAllOrdersAsync(OrderFilter filter, PagedQuery query,CancellationToken ct);
}