using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class BannersController(IBannerService BannerService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddBannerAsync(BannerInsertDto Banner)
    {
        return await BannerService.AddBannerAsync(Banner);
    }
    [HttpPut("{BannerId}")]
    public async Task<Response<string>> UpdateAsync(int BannerId,BannerUpdateDto Banner)
    {
        return await BannerService.UpdateAsync(BannerId,Banner);
    }
    [HttpDelete("{BannerId}")]
    public async Task<Response<string>> DeleteAsync(int BannerId)
    {
        return await BannerService.DeleteAsync(BannerId);
    }
    [HttpGet]
    public async Task<PagedResult<BannerGetDto>> GetAllBanners([FromQuery] BannerFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await BannerService.GetAllBannersAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{BannerId}")]
    public async Task<Response<BannerGetDto>> GetBannerByIdAsync(int BannerId)
    {
        return await BannerService.GetBannerByIdAsync(BannerId);
    }
}