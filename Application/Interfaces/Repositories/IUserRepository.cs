using WebApi.DTOs;

public interface IUserRepository
{
    Task AddAsync(ApplicationUser ApplicationUser);
    Task<ApplicationUser?> GetByIdAsync(int id);
    Task DeleteAsync(int ApplicationUser);
    Task UpdateAsync(ApplicationUser ApplicationUser);
    Task<PagedResult<ApplicationUser>> GetAllUsersAsync(UserFilter filter, PagedQuery query,CancellationToken ct);
}