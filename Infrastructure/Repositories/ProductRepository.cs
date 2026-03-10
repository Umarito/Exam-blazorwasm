using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class ProductRepository(ApplicationDbContext applicationDBContext,ILogger<ProductRepository> logger) : IProductRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<ProductRepository> _logger = logger;

    public async Task AddAsync(Product Product)
    {
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int ProductId)
    {
        var delete = await _context.Products.FindAsync(ProductId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Product Product)
    {
        _context.Products.Update(Product);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Product>> GetAllProductsAsync(ProductFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Product> query = _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Reviews);

        if (!string.IsNullOrWhiteSpace(filter?.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter?.IsActive != null)
            query = query.Where(x => x.IsActive == filter.IsActive); 

        if (filter?.Price > 0)
            query = query.Where(x => x.Price <= filter.Price);

        if (filter?.MinPrice != null)
            query = query.Where(x => x.Price >= filter.MinPrice);

        if (filter?.MaxPrice != null)
            query = query.Where(x => x.Price <= filter.MaxPrice);

        if (filter?.CategoryId != null)
            query = query.Where(x => x.CategoryId == filter.CategoryId);

        if (filter?.BrandId != null)
            query = query.Where(x => x.BrandId == filter.BrandId);

        if (filter?.IsAvailable != null)
            query = filter.IsAvailable.Value
                ? query.Where(x => x.StockQuantity > 0)
                : query.Where(x => x.StockQuantity <= 0);

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Product>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
