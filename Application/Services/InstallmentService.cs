using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class InstallmentService(IMapper mapper,IInstallmentRepository InstallmentRepository,ILogger<InstallmentService> logger,IMemoryCache cache) : IInstallmentService
{
    private readonly IInstallmentRepository _InstallmentRepository = InstallmentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<InstallmentService> _logger = logger;
    private string BuildCacheKey(InstallmentFilter filter, PagedQuery query)
    {
        return $"Installments:getAll:page={query.Page}:size={query.PageSize}:monthCount={filter?.MonthCount}:interestRate={filter?.InterestRate}";
    }
    public async Task<Response<string>> AddInstallmentAsync(InstallmentInsertDto InstallmentInsertDto)
    {
        try
        {
            var Installment = _mapper.Map<Installment>(InstallmentInsertDto);
            await _InstallmentRepository.AddAsync(Installment);
            return new Response<string>(HttpStatusCode.OK, "Installment was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int InstallmentId)
    {
        try
        {
            await _InstallmentRepository.DeleteAsync(InstallmentId);
            return new Response<string>(HttpStatusCode.OK, "Installment was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<PagedResult<InstallmentGetDto>> GetAllInstallmentsAsync(InstallmentFilter filter,PagedQuery query,CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(filter,query);

        if (_cache.TryGetValue(cacheKey, out PagedResult<InstallmentGetDto> cachedData))
        {
            _logger.LogInformation("Cache HIT");
            return cachedData;
        }

        _logger.LogInformation("Cache MISS");

        var Installments = await _InstallmentRepository.GetAllInstallmentsAsync(filter, query, ct);

        var res = _mapper.Map<PagedResult<InstallmentGetDto>>(Installments);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.High);

        _cache.Set(cacheKey, res, cacheOptions);

        return res;
    }

    public async Task<Response<InstallmentGetDto>> GetInstallmentByIdAsync(int InstallmentId)
    {
        try
        {
            var Installment = await _InstallmentRepository.GetByIdAsync(InstallmentId);
            var res = _mapper.Map<InstallmentGetDto>(Installment);
            return new Response<InstallmentGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<InstallmentGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int InstallmentId, InstallmentUpdateDto Installment)
    {
        try
        {
            var res = await _InstallmentRepository.GetByIdAsync(InstallmentId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"Installment not found");
            }
            else
            {
                _mapper.Map(Installment, res);
                await _InstallmentRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"Installment updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}