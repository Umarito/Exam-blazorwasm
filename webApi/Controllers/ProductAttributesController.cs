using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ProductAttributesController(IProductAttributeService ProductAttributeService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddProductAttributeAsync(ProductAttributeInsertDto ProductAttribute)
    {
        return await ProductAttributeService.AddProductAttributeAsync(ProductAttribute);
    }
    [HttpPut("{ProductAttributeId}")]
    public async Task<Response<string>> UpdateAsync(int ProductAttributeId,ProductAttributeUpdateDto ProductAttribute)
    {
        return await ProductAttributeService.UpdateAsync(ProductAttributeId,ProductAttribute);
    }
    [HttpDelete("{ProductAttributeId}")]
    public async Task<Response<string>> DeleteAsync(int ProductAttributeId)
    {
        return await ProductAttributeService.DeleteAsync(ProductAttributeId);
    }
    [HttpGet]
    public async Task<PagedResult<ProductAttributeGetDto>> GetAllProductAttributes([FromQuery] ProductAttributeFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await ProductAttributeService.GetAllProductAttributesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{ProductAttributeId}")]
    public async Task<Response<ProductAttributeGetDto>> GetProductAttributeByIdAsync(int ProductAttributeId)
    {
        return await ProductAttributeService.GetProductAttributeByIdAsync(ProductAttributeId);
    }
}