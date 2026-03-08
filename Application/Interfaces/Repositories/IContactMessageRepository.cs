using WebApi.DTOs;

public interface IContactMessageRepository
{
    Task AddAsync(ContactMessages ContactMessage);
    Task<ContactMessages?> GetByIdAsync(int id);
    Task DeleteAsync(int ContactMessage);
    Task UpdateAsync(ContactMessages ContactMessage);
    Task<List<ContactMessages>> GetAllContactMessagesAsync();
}