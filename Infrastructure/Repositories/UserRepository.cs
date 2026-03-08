using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class UserRepository(ApplicationDbContext applicationDBContext,ILogger<UserRepository> logger) : IUserRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<UserRepository> _logger = logger;

    public async Task AddAsync(ApplicationUser User)
    {
        _context.Users.Add(User);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int UserId)
    {
        var delete = await _context.Users.FindAsync(UserId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<ApplicationUser?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task UpdateAsync(ApplicationUser User)
    {
        _context.Users.Update(User);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<ApplicationUser>> GetAllUsersAsync(UserFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<ApplicationUser> query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.FullName))
            query = query.Where(x => x.FullName.Contains(filter.FullName));

        if (!string.IsNullOrWhiteSpace(filter?.Phone))
            query = query.Where(x => x.PhoneNumber.Contains(filter.Phone));  

        if (!string.IsNullOrWhiteSpace(filter?.Email))
            query = query.Where(x => x.Email.Contains(filter.Email));

        if (filter?.IsActive != null)
            query = query.Where(x => x.IsActive == filter.IsActive);  
        
        if (filter?.Role != null)
        {
            query = query.Where(x => x.Role == filter.Role);
        }

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<ApplicationUser>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}