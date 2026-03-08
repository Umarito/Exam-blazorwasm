using WebApi.DTOs;

public interface IReviewService
{
    Task<Response<string>> AddReviewAsync(ReviewInsertDto ReviewInsertDto);
    Task<Response<ReviewGetDto>> GetReviewByIdAsync(int ReviewId);
    Task<PagedResult<ReviewGetDto>> GetAllReviewsAsync(ReviewFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int ReviewId);
    Task<Response<string>> UpdateAsync(int ReviewId,ReviewUpdateDto ReviewUpdateDto);
}