using WebApi.DTOs;

public interface IInstallmentService
{
    Task<Response<string>> AddInstallmentAsync(InstallmentInsertDto InstallmentInsertDto);
    Task<Response<InstallmentGetDto>> GetInstallmentByIdAsync(int InstallmentId);
    Task<PagedResult<InstallmentGetDto>> GetAllInstallmentsAsync(InstallmentFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int InstallmentId);
    Task<Response<string>> UpdateAsync(int InstallmentId,InstallmentUpdateDto InstallmentUpdateDto);
}