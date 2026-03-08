using WebApi.DTOs;

public interface IOrderItemService
{
    Task<Response<string>> AddOrderItemAsync(OrderItemInsertDto OrderItemInsertDto);
    Task<Response<OrderItemGetDto>> GetOrderItemByIdAsync(int OrderItemId);
    Task<PagedResult<OrderItemGetDto>> GetAllOrderItemsAsync(OrderItemFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int OrderItemId);
    Task<Response<string>> UpdateAsync(int OrderItemId,OrderItemUpdateDto OrderItemUpdateDto);
}