using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class ProductAttributeRepository(ApplicationDbContext applicationDBContext,ILogger<ProductAttributeRepository> logger) : IProductAttributeRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<ProductAttributeRepository> _logger = logger;

    public async Task AddAsync(ProductAttribute ProductAttribute)
    {
        _context.ProductAttributes.Add(ProductAttribute);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int ProductAttributeId)
    {
        var delete = await _context.ProductAttributes.FindAsync(ProductAttributeId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductAttribute?> GetByIdAsync(int id)
    {
        return await _context.ProductAttributes.FindAsync(id);
    }

    public async Task UpdateAsync(ProductAttribute ProductAttribute)
    {
        _context.ProductAttributes.Update(ProductAttribute);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<ProductAttribute>> GetAllProductAttributesAsync(ProductAttributeFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<ProductAttribute> query = _context.ProductAttributes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Value))
            query = query.Where(x => x.Value.Contains(filter.Value));  

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<ProductAttribute>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}