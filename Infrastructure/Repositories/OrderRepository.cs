using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class OrderRepository(ApplicationDbContext applicationDBContext,ILogger<OrderRepository> logger) : IOrderRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<OrderRepository> _logger = logger;

    public async Task AddAsync(Order Order)
    {
        _context.Orders.Add(Order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int OrderId)
    {
        var delete = await _context.Orders.FindAsync(OrderId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateAsync(Order Order)
    {
        _context.Orders.Update(Order);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Order>> GetAllOrdersAsync(OrderFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Order> query = _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product);

        if (!string.IsNullOrWhiteSpace(filter?.Phone))
            query = query.Where(x => x.Phone.Contains(filter.Phone));

        if (!string.IsNullOrWhiteSpace(filter?.DeliveryAddress))
            query = query.Where(x => x.DeliveryAddress.Contains(filter.DeliveryAddress));

        if (filter?.Status.HasValue == true)
            query = query.Where(x => x.Status == filter.Status.Value); 

        if (filter?.TotalAmount > 0)
            query = query.Where(x => x.TotalAmount <= filter.TotalAmount);

        if (!string.IsNullOrWhiteSpace(filter?.UserId))
            query = query.Where(x => x.UserId == filter.UserId);

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Order>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
