using WebApi.DTOs;

public interface IOrderService
{
    Task<Response<int>> AddOrderAsync(OrderInsertDto OrderInsertDto);
    Task<Response<OrderGetDto>> GetOrderByIdAsync(int OrderId);
    Task<PagedResult<OrderGetDto>> GetAllOrdersAsync(OrderFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int OrderId);
    Task<Response<string>> UpdateAsync(int OrderId,OrderUpdateDto OrderUpdateDto);
}
