using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class StoreLocationRepository(ApplicationDbContext applicationDBContext,ILogger<StoreLocationRepository> logger) : IStoreLocationRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<StoreLocationRepository> _logger = logger;

    public async Task AddAsync(StoreLocation StoreLocation)
    {
        _context.StoreLocations.Add(StoreLocation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int StoreLocationId)
    {
        var delete = await _context.StoreLocations.FindAsync(StoreLocationId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<StoreLocation?> GetByIdAsync(int id)
    {
        return await _context.StoreLocations.FindAsync(id);
    }

    public async Task UpdateAsync(StoreLocation StoreLocation)
    {
        _context.StoreLocations.Update(StoreLocation);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<StoreLocation>> GetAllStoreLocationsAsync(StoreLocationFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<StoreLocation> query = _context.StoreLocations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Address))
            query = query.Where(x => x.Address.Contains(filter.Address));

        if (!string.IsNullOrWhiteSpace(filter?.MapCoordinates))
            query = query.Where(x => x.MapCoordinates.Contains(filter.MapCoordinates));

        if (!string.IsNullOrWhiteSpace(filter?.WorkingHours))
            query = query.Where(x => x.WorkingHours.Contains(filter.WorkingHours));

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<StoreLocation>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}