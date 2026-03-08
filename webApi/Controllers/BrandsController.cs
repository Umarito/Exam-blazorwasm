using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class BrandsController(IBrandService BrandService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddBrandAsync(BrandInsertDto Brand)
    {
        return await BrandService.AddBrandAsync(Brand);
    }
    [HttpPut("{BrandId}")]
    public async Task<Response<string>> UpdateAsync(int BrandId,BrandUpdateDto Brand)
    {
        return await BrandService.UpdateAsync(BrandId,Brand);
    }
    [HttpDelete("{BrandId}")]
    public async Task<Response<string>> DeleteAsync(int BrandId)
    {
        return await BrandService.DeleteAsync(BrandId);
    }
    [HttpGet]
    public async Task<PagedResult<BrandGetDto>> GetAllBrands([FromQuery] BrandFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await BrandService.GetAllBrandsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{BrandId}")]
    public async Task<Response<BrandGetDto>> GetBrandByIdAsync(int BrandId)
    {
        return await BrandService.GetBrandByIdAsync(BrandId);
    }
}