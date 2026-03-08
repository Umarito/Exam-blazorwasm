using WebApi.DTOs;

public interface IAttributeRepository
{
    Task AddAsync(Attribute Attribute);
    Task<Attribute?> GetByIdAsync(int id);
    Task DeleteAsync(int Attribute);
    Task UpdateAsync(Attribute Attribute);
    Task<PagedResult<Attribute>> GetAllAttributesAsync(AttributeFilter filter, PagedQuery query,CancellationToken ct);
}