using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AttributesController(IAttributeService AttributeService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddAttributeAsync(AttributeInsertDto Attribute)
    {
        return await AttributeService.AddAttributeAsync(Attribute);
    }
    [HttpPut("{AttributeId}")]
    public async Task<Response<string>> UpdateAsync(int AttributeId,AttributeUpdateDto Attribute)
    {
        return await AttributeService.UpdateAsync(AttributeId,Attribute);
    }
    [HttpDelete("{AttributeId}")]
    public async Task<Response<string>> DeleteAsync(int AttributeId)
    {
        return await AttributeService.DeleteAsync(AttributeId);
    }
    [HttpGet]
    public async Task<PagedResult<AttributeGetDto>> GetAllAttributes([FromQuery] AttributeFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await AttributeService.GetAllAttributesAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{AttributeId}")]
    public async Task<Response<AttributeGetDto>> GetAttributeByIdAsync(int AttributeId)
    {
        return await AttributeService.GetAttributeByIdAsync(AttributeId);
    }
}