using WebApi.DTOs;

public interface IAttributeService
{
    Task<Response<string>> AddAttributeAsync(AttributeInsertDto AttributeInsertDto);
    Task<Response<AttributeGetDto>> GetAttributeByIdAsync(int AttributeId);
    Task<PagedResult<AttributeGetDto>> GetAllAttributesAsync(AttributeFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int AttributeId);
    Task<Response<string>> UpdateAsync(int AttributeId,AttributeUpdateDto AttributeUpdateDto);
}