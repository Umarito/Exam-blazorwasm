using WebApi.DTOs;

public interface IReviewRepository
{
    Task AddAsync(Review Review);
    Task<Review?> GetByIdAsync(int id);
    Task DeleteAsync(int Review);
    Task UpdateAsync(Review Review);
    Task<PagedResult<Review>> GetAllReviewsAsync(ReviewFilter filter, PagedQuery query,CancellationToken ct);
}