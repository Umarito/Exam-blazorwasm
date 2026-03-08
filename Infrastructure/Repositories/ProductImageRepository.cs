using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class ProductImageRepository(ApplicationDbContext applicationDBContext,ILogger<ProductImageRepository> logger) : IProductImageRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<ProductImageRepository> _logger = logger;

    public async Task AddAsync(ProductImage ProductImage)
    {
        _context.ProductImages.Add(ProductImage);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int ProductImageId)
    {
        var delete = await _context.ProductImages.FindAsync(ProductImageId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage?> GetByIdAsync(int id)
    {
        return await _context.ProductImages.FindAsync(id);
    }

    public async Task UpdateAsync(ProductImage ProductImage)
    {
        _context.ProductImages.Update(ProductImage);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<ProductImage>> GetAllProductImagesAsync(ProductImageFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<ProductImage> query = _context.ProductImages.AsNoTracking();

        if (filter?.IsMain != null)
            query = query.Where(x => x.IsMain == filter.IsMain); 

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<ProductImage>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}