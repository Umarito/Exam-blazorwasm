using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class InstallmentsController(IInstallmentService InstallmentService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddInstallmentAsync(InstallmentInsertDto Installment)
    {
        return await InstallmentService.AddInstallmentAsync(Installment);
    }
    [HttpPut("{InstallmentId}")]
    public async Task<Response<string>> UpdateAsync(int InstallmentId,InstallmentUpdateDto Installment)
    {
        return await InstallmentService.UpdateAsync(InstallmentId,Installment);
    }
    [HttpDelete("{InstallmentId}")]
    public async Task<Response<string>> DeleteAsync(int InstallmentId)
    {
        return await InstallmentService.DeleteAsync(InstallmentId);
    }
    [HttpGet]
    public async Task<PagedResult<InstallmentGetDto>> GetAllInstallments([FromQuery] InstallmentFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await InstallmentService.GetAllInstallmentsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{InstallmentId}")]
    public async Task<Response<InstallmentGetDto>> GetInstallmentByIdAsync(int InstallmentId)
    {
        return await InstallmentService.GetInstallmentByIdAsync(InstallmentId);
    }
}