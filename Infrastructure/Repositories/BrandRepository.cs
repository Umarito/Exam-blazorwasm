using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class BrandRepository(ApplicationDbContext applicationDBContext,ILogger<BrandRepository> logger) : IBrandRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<BrandRepository> _logger = logger;

    public async Task AddAsync(Brand Brand)
    {
        _context.Brands.Add(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int BrandId)
    {
        var delete = await _context.Brands.FindAsync(BrandId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task UpdateAsync(Brand Brand)
    {
        _context.Brands.Update(Brand);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Brand>> GetAllBrandsAsync(BrandFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Brand> query = _context.Brands.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter?.Slug))
            query = query.Where(x => x.Slug.Contains(filter.Slug));  

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Brand>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}