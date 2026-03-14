using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class BannerRepository(ApplicationDbContext applicationDBContext) : IBannerRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    // private readonly ILogger<BannerRepository> _logger = logger;

    public async Task AddAsync(Banner Banner)
    {
        _context.Banners.Add(Banner);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int BannerId)
    {
        var delete = await _context.Banners.FindAsync(BannerId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Banner?> GetByIdAsync(int id)
    {
        return await _context.Banners.FindAsync(id);
    }

    public async Task UpdateAsync(Banner Banner)
    {
        _context.Banners.Update(Banner);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Banner>> GetAllBannersAsync(BannerFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Banner> query = _context.Banners.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.ImageUrl))
            query = query.Where(x => x.ImageUrl.Contains(filter.ImageUrl));  

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Banner>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}