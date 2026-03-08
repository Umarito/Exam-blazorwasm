using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class ReviewRepository(ApplicationDbContext applicationDBContext,ILogger<ReviewRepository> logger) : IReviewRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<ReviewRepository> _logger = logger;

    public async Task AddAsync(Review Review)
    {
        _context.Reviews.Add(Review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int ReviewId)
    {
        var delete = await _context.Reviews.FindAsync(ReviewId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task UpdateAsync(Review Review)
    {
        _context.Reviews.Update(Review);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Review>> GetAllReviewsAsync(ReviewFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Review> query = _context.Reviews.AsNoTracking();

        if (filter?.IsApproved != null)
            query = query.Where(x => x.IsApproved == filter.IsApproved); 

        if (filter?.Rating > 0)
            query = query.Where(x => x.Rating <= filter.Rating);

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Review>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}