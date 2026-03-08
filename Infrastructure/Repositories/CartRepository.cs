using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class CartRepository(ApplicationDbContext applicationDBContext,ILogger<CartRepository> logger) : ICartRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<CartRepository> _logger = logger;

    public async Task AddAsync(Cart Cart)
    {
        _context.Carts.Add(Cart);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int CartId)
    {
        var delete = await _context.Carts.FindAsync(CartId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Carts.FindAsync(id);
    }

    public async Task UpdateAsync(Cart Cart)
    {
        _context.Carts.Update(Cart);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Cart>> GetAllCartsAsync()
    {
        return await _context.Carts.ToListAsync();
    }
}