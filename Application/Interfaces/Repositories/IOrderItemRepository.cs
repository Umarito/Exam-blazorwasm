using WebApi.DTOs;

public interface IOrderItemRepository
{
    Task AddAsync(OrderItem OrderItem);
    Task<OrderItem?> GetByIdAsync(int id);
    Task DeleteAsync(int OrderItem);
    Task UpdateAsync(OrderItem OrderItem);
    Task<PagedResult<OrderItem>> GetAllOrderItemsAsync(OrderItemFilter filter, PagedQuery query,CancellationToken ct);
}