using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class CityRepository(ApplicationDbContext applicationDBContext,ILogger<CityRepository> logger) : ICityRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<CityRepository> _logger = logger;

    public async Task AddAsync(City City)
    {
        _context.Cities.Add(City);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int CityId)
    {
        var delete = await _context.Cities.FindAsync(CityId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task UpdateAsync(City City)
    {
        _context.Cities.Update(City);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<City>> GetAllCitiesAsync(CityFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<City> query = _context.Cities.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));  

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<City>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}