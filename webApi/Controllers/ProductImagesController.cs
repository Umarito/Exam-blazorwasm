using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ProductImagesController(IProductImageService ProductImageService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddProductImageAsync(ProductImageInsertDto ProductImage)
    {
        return await ProductImageService.AddProductImageAsync(ProductImage);
    }
    [HttpPut("{ProductImageId}")]
    public async Task<Response<string>> UpdateAsync(int ProductImageId,ProductImageUpdateDto ProductImage)
    {
        return await ProductImageService.UpdateAsync(ProductImageId,ProductImage);
    }
    [HttpDelete("{ProductImageId}")]
    public async Task<Response<string>> DeleteAsync(int ProductImageId)
    {
        return await ProductImageService.DeleteAsync(ProductImageId);
    }
    [HttpGet]
    public async Task<PagedResult<ProductImageGetDto>> GetAllProductImages([FromQuery] ProductImageFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await ProductImageService.GetAllProductImagesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{ProductImageId}")]
    public async Task<Response<ProductImageGetDto>> GetProductImageByIdAsync(int ProductImageId)
    {
        return await ProductImageService.GetProductImageByIdAsync(ProductImageId);
    }
}