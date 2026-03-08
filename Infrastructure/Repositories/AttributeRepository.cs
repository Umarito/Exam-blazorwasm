using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class AttributeRepository(ApplicationDbContext applicationDBContext,ILogger<AttributeRepository> logger) : IAttributeRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<AttributeRepository> _logger = logger;

    public async Task AddAsync(Attribute Attribute)
    {
        _context.Attributes.Add(Attribute);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int AttributeId)
    {
        var delete = await _context.Attributes.FindAsync(AttributeId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Attribute?> GetByIdAsync(int id)
    {
        return await _context.Attributes.FindAsync(id);
    }

    public async Task UpdateAsync(Attribute Attribute)
    {
        _context.Attributes.Update(Attribute);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Attribute>> GetAllAttributesAsync(AttributeFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Attribute> query = _context.Attributes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));  

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Attribute>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}