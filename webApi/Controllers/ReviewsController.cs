using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewService ReviewService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddReviewAsync(ReviewInsertDto Review)
    {
        return await ReviewService.AddReviewAsync(Review);
    }
    [HttpPut("{ReviewId}")]
    public async Task<Response<string>> UpdateAsync(int ReviewId,ReviewUpdateDto Review)
    {
        return await ReviewService.UpdateAsync(ReviewId,Review);
    }
    [HttpDelete("{ReviewId}")]
    public async Task<Response<string>> DeleteAsync(int ReviewId)
    {
        return await ReviewService.DeleteAsync(ReviewId);
    }
    [HttpGet]
    public async Task<PagedResult<ReviewGetDto>> GetAllReviews([FromQuery] ReviewFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await ReviewService.GetAllReviewsAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{ReviewId}")]
    public async Task<Response<ReviewGetDto>> GetReviewByIdAsync(int ReviewId)
    {
        return await ReviewService.GetReviewByIdAsync(ReviewId);
    }
}