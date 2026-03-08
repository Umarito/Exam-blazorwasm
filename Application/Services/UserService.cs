using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class UserService(IMapper mapper,IUserRepository UserRepository,ILogger<UserService> logger,IMemoryCache cache) : IUserService
{
    private readonly IUserRepository _UserRepository = UserRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<UserService> _logger = logger;
    private string BuildCacheKey(UserFilter filter, PagedQuery query)
    {
        return $"Users:getAll:page={query.Page}:size={query.PageSize}:fullName={filter?.FullName}:email={filter?.Email}:isActive={filter?.IsActive}:phone={filter?.Phone}:role={filter?.Role}";
    }
    public async Task<Response<string>> AddUserAsync(UserInsertDto UserInsertDto)
    {
        try
        {
            var User = _mapper.Map<ApplicationUser>(UserInsertDto);
            await _UserRepository.AddAsync(User);
            return new Response<string>(HttpStatusCode.OK, "User was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int UserId)
    {
        try
        {
            await _UserRepository.DeleteAsync(UserId);
            return new Response<string>(HttpStatusCode.OK, "User was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<UserGetDto>> GetAllUsersAsync(UserFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<UserGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Users = await _UserRepository.GetAllUsersAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<UserGetDto>>(Users);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<UserGetDto>> GetUserByIdAsync(int UserId)
    {
        try
        {
            var User = await _UserRepository.GetByIdAsync(UserId);
            var res = _mapper.Map<UserGetDto>(User);
            return new Response<UserGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<UserGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int UserId, UserUpdateDto User)
    {
        try
        {
            var res = await _UserRepository.GetByIdAsync(UserId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"User not found");
            }
            else
            {
                _mapper.Map(User, res);
                await _UserRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"User updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}