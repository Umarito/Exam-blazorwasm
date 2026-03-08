using WebApi.DTOs;

public interface IPageRepository
{
    Task AddAsync(PageSMS PageSMS);
    Task<PageSMS?> GetByIdAsync(int id);
    Task DeleteAsync(int PageSMS);
    Task UpdateAsync(PageSMS PageSMS);
    Task<PagedResult<PageSMS>> GetAllPagesAsync(PageSMSFilter filter, PagedQuery query,CancellationToken ct);
}