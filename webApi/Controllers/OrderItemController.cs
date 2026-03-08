using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsController(IOrderItemService OrderItemService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddOrderItemAsync(OrderItemInsertDto OrderItem)
    {
        return await OrderItemService.AddOrderItemAsync(OrderItem);
    }
    [HttpPut("{OrderItemId}")]
    public async Task<Response<string>> UpdateAsync(int OrderItemId,OrderItemUpdateDto OrderItem)
    {
        return await OrderItemService.UpdateAsync(OrderItemId,OrderItem);
    }
    [HttpDelete("{OrderItemId}")]
    public async Task<Response<string>> DeleteAsync(int OrderItemId)
    {
        return await OrderItemService.DeleteAsync(OrderItemId);
    }
    [HttpGet]
    public async Task<PagedResult<OrderItemGetDto>> GetAllOrderItems([FromQuery] OrderItemFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await OrderItemService.GetAllOrderItemsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{OrderItemId}")]
    public async Task<Response<OrderItemGetDto>> GetOrderItemByIdAsync(int OrderItemId)
    {
        return await OrderItemService.GetOrderItemByIdAsync(OrderItemId);
    }
}