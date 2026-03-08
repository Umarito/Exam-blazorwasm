using WebApi.DTOs;

public interface IUserService
{
    Task<Response<string>> AddUserAsync(UserInsertDto UserInsertDto);
    Task<Response<UserGetDto>> GetUserByIdAsync(int UserId);
    Task<PagedResult<UserGetDto>> GetAllUsersAsync(UserFilter filter, PagedQuery query,CancellationToken ct);
    Task<Response<string>> DeleteAsync(int UserId);
    Task<Response<string>> UpdateAsync(int UserId,UserUpdateDto UserUpdateDto);
}