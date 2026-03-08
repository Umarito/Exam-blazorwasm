using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class CategoryRepository(ApplicationDbContext applicationDBContext,ILogger<CategoryRepository> logger) : ICategoryRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<CategoryRepository> _logger = logger;

    public async Task AddAsync(Category Category)
    {
        _context.Categories.Add(Category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int CategoryId)
    {
        var delete = await _context.Categories.FindAsync(CategoryId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task UpdateAsync(Category Category)
    {
        _context.Categories.Update(Category);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Category>> GetAllCategoriesAsync(CategoryFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Category> query = _context.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter?.Slug))
            query = query.Where(x => x.Slug.Contains(filter.Slug));

        if (filter?.IsActive != null)
            query = query.Where(x => x.IsActive == filter.IsActive); 

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Category>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}