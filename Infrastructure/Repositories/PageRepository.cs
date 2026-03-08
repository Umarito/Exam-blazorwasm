using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class PageSMSRepository(ApplicationDbContext applicationDBContext,ILogger<PageSMSRepository> logger) : IPageRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<PageSMSRepository> _logger = logger;

    public async Task AddAsync(PageSMS PageSMS)
    {
        _context.PageSMSs.Add(PageSMS);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int PageSMSId)
    {
        var delete = await _context.PageSMSs.FindAsync(PageSMSId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<PageSMS?> GetByIdAsync(int id)
    {
        return await _context.PageSMSs.FindAsync(id);
    }

    public async Task UpdateAsync(PageSMS PageSMS)
    {
        _context.PageSMSs.Update(PageSMS);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<PageSMS>> GetAllPagesAsync(PageSMSFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<PageSMS> query = _context.PageSMSs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Title))
            query = query.Where(x => x.Title.Contains(filter.Title));

        if (!string.IsNullOrWhiteSpace(filter?.Slug))
            query = query.Where(x => x.Slug.Contains(filter.Slug));

        if (filter?.IsPublished != null)
            query = query.Where(x => x.IsPublished == filter.IsPublished); 

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<PageSMS>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}