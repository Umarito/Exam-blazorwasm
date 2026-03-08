using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService UserService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddUserAsync(UserInsertDto User)
    {
        return await UserService.AddUserAsync(User);
    }
    [HttpPut("{UserId}")]
    public async Task<Response<string>> UpdateAsync(int UserId,UserUpdateDto User)
    {
        return await UserService.UpdateAsync(UserId,User);
    }
    [HttpDelete("{UserId}")]
    public async Task<Response<string>> DeleteAsync(int UserId)
    {
        return await UserService.DeleteAsync(UserId);
    }
    [HttpGet]
    public async Task<PagedResult<UserGetDto>> GetAllUsers([FromQuery] UserFilter filter, [FromQuery] PagedQuery pagedQuery,CancellationToken token)
    {
        return await UserService.GetAllUsersAsync(filter, pagedQuery,token);   
    }
    
    [HttpGet("{UserId}")]
    public async Task<Response<UserGetDto>> GetUserByIdAsync(int UserId)
    {
        return await UserService.GetUserByIdAsync(UserId);
    }
}