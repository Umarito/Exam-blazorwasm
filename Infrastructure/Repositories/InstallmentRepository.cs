using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class InstallmentRepository(ApplicationDbContext applicationDBContext,ILogger<InstallmentRepository> logger) : IInstallmentRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<InstallmentRepository> _logger = logger;

    public async Task AddAsync(Installment Installment)
    {
        _context.Installments.Add(Installment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int InstallmentId)
    {
        var delete = await _context.Installments.FindAsync(InstallmentId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Installment?> GetByIdAsync(int id)
    {
        return await _context.Installments.FindAsync(id);
    }

    public async Task UpdateAsync(Installment Installment)
    {
        _context.Installments.Update(Installment);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<Installment>> GetAllInstallmentsAsync(InstallmentFilter filter, PagedQuery pagedQuery,CancellationToken ct)
    {
        var page = pagedQuery.Page <= 0 ? 1 : pagedQuery.Page;
        var pageSize = pagedQuery.PageSize <= 0 ? 10 : pagedQuery.PageSize;

        IQueryable<Installment> query = _context.Installments.AsNoTracking();

        if (filter?.MonthCount > 0)
        query = query.Where(x => x.MonthCount <= filter.MonthCount);

        if (filter?.InterestRate > 0)
        query = query.Where(x => x.InterestRate <= filter.InterestRate);

        var totalCount = await query.CountAsync();

        query = query
            .OrderBy(x => x.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Installment>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}