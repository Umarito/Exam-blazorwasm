using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class PagesController(IPageService PageService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddPageAsync(PageInsertDto Page)
    {
        return await PageService.AddPageAsync(Page);
    }
    [HttpPut("{PageId}")]
    public async Task<Response<string>> UpdateAsync(int PageId,PageUpdateDto Page)
    {
        return await PageService.UpdateAsync(PageId,Page);
    }
    [HttpDelete("{PageId}")]
    public async Task<Response<string>> DeleteAsync(int PageId)
    {
        return await PageService.DeleteAsync(PageId);
    }
    [HttpGet]
    public async Task<PagedResult<PageGetDto>> GetAllPages([FromQuery] PageSMSFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await PageService.GetAllPagesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{PageId}")]
    public async Task<Response<PageGetDto>> GetPageByIdAsync(int PageId)
    {
        return await PageService.GetPageByIdAsync(PageId);
    }
}