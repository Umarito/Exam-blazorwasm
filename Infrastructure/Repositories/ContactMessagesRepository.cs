using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.DTOs;

public class ContactMessagesRepository(ApplicationDbContext applicationDBContext,ILogger<ContactMessagesRepository> logger) : IContactMessageRepository
{
    private readonly ApplicationDbContext _context = applicationDBContext;
    private readonly ILogger<ContactMessagesRepository> _logger = logger;

    public async Task AddAsync(ContactMessages ContactMessages)
    {
        _context.ContactMessages.Add(ContactMessages);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int ContactMessagesId)
    {
        var delete = await _context.ContactMessages.FindAsync(ContactMessagesId);
        _context.RemoveRange(delete);
        await _context.SaveChangesAsync();
    }

    public async Task<ContactMessages?> GetByIdAsync(int id)
    {
        return await _context.ContactMessages.FindAsync(id);
    }

    public async Task UpdateAsync(ContactMessages ContactMessages)
    {
        _context.ContactMessages.Update(ContactMessages);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ContactMessages>> GetAllContactMessagesAsync()
    {
        return await _context.ContactMessages.ToListAsync();
    }
}