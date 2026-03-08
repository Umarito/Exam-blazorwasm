using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class CartItemRepository(ApplicationDbContext applicationDBContext,ILogger<CartItemRepository> logger) : ICartItemRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<CartItemRepository> _logger = logger;

    public async Task AddAsync(CartItem CartItem)
    {
        _context.CartItems.Add(CartItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int CartItemId)
    {
        var delete = await _context.CartItems.FindAsync(CartItemId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        return await _context.CartItems.FindAsync(id);
    }

    public async Task UpdateAsync(CartItem CartItem)
    {
        _context.CartItems.Update(CartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<CartItem>> GetAllCartItemsAsync(CartItemFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<CartItem> query = _context.CartItems.AsNoTracking();

        if (filter?.Quantity != 0)
            query = query.Where(x => x.Quantity <= filter.Quantity); 

        if (filter?.UnitPrice != 0)
            query = query.Where(x => x.UnitPrice <= filter.UnitPrice); 

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<CartItem>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}