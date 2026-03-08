using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class WishlistRepository(ApplicationDbContext applicationDBContext,ILogger<WishlistRepository> logger) : IWishlistRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<WishlistRepository> _logger = logger;

    public async Task AddAsync(Wishlist Wishlist)
    {
        _context.Wishlists.Add(Wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int WishlistId)
    {
        var delete = await _context.Wishlists.FindAsync(WishlistId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<Wishlist?> GetByIdAsync(int id)
    {
        return await _context.Wishlists.FindAsync(id);
    }

    public async Task UpdateAsync(Wishlist Wishlist)
    {
        _context.Wishlists.Update(Wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Wishlist>> GetAllWishlistsAsync()
    {
        return await _context.Wishlists.ToListAsync();
    }
}