using WebApi.DTOs;

public interface IInstallmentRepository
{
    Task AddAsync(Installment Installment);
    Task<Installment?> GetByIdAsync(int id);
    Task DeleteAsync(int Installment);
    Task UpdateAsync(Installment Installment);
    Task<PagedResult<Installment>> GetAllInstallmentsAsync(InstallmentFilter filter, PagedQuery query,CancellationToken ct);
}