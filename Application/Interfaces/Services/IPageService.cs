using WebApi.DTOs;

public interface IPageService
{
    Task<Response<string>> AddPageAsync(PageInsertDto PageInsertDto);
    Task<Response<PageGetDto>> GetPageByIdAsync(int PageId);
    Task<PagedResult<PageGetDto>> GetAllPagesAsync(PageSMSFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int PageId);
    Task<Response<string>> UpdateAsync(int PageId,PageUpdateDto PageUpdateDto);
}