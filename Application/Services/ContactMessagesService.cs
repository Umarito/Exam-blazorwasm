using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

public class ContactMessageService(IMapper mapper,IContactMessageRepository ContactMessageRepository,ILogger<ContactMessageService> logger,IMemoryCache cache) : IContactMessageService
{
    private readonly IContactMessageRepository _ContactMessageRepository = ContactMessageRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<ContactMessageService> _logger = logger;
    public async Task<Response<string>> AddContactMessageAsync(ContactMessageInsertDto ContactMessageInsertDto)
    {
        try
        {
            var ContactMessage = _mapper.Map<ContactMessages>(ContactMessageInsertDto);
            await _ContactMessageRepository.AddAsync(ContactMessage);
            return new Response<string>(HttpStatusCode.OK, "ContactMessage was added successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> DeleteAsync(int ContactMessageId)
    {
        try
        {
            await _ContactMessageRepository.DeleteAsync(ContactMessageId);
            return new Response<string>(HttpStatusCode.OK, "ContactMessage was deleted successfully");
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<List<ContactMessageGetDto>>> GetAllContactMessagesAsync()
    {
        try
        {
            var messages = _ContactMessageRepository.GetAllContactMessagesAsync();
            if(messages == null)
            {
                return new Response<List<ContactMessageGetDto>>(HttpStatusCode.NotFound,"There is no contact messages");
            }
            else
            {
                var mapped = _mapper.Map<List<ContactMessageGetDto>>(messages);
                return new Response<List<ContactMessageGetDto>>(HttpStatusCode.OK,"ok",mapped);
            }
        }
        catch(Exception ex)
        {
            return new Response<List<ContactMessageGetDto>>(HttpStatusCode.InternalServerError,ex.Message);
        }
    }

    public async Task<Response<ContactMessageGetDto>> GetContactMessageByIdAsync(int ContactMessageId)
    {
        try
        {
            var ContactMessage = await _ContactMessageRepository.GetByIdAsync(ContactMessageId);
            var res = _mapper.Map<ContactMessageGetDto>(ContactMessage);
            return new Response<ContactMessageGetDto>(HttpStatusCode.OK,"The data that you were searching for:",res);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex.Message);
            return new Response<ContactMessageGetDto>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAsync(int ContactMessageId, ContactMessageUpdateDto ContactMessage)
    {
        try
        {
            var res = await _ContactMessageRepository.GetByIdAsync(ContactMessageId);

            if (res == null)
            {   
                return new Response<string>(HttpStatusCode.NotFound,"ContactMessage not found");
            }
            else
            {
                _mapper.Map(ContactMessage, res);
                await _ContactMessageRepository.UpdateAsync(res);
                return new Response<string>(HttpStatusCode.OK,"ContactMessage updated successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError,"Internal Server Error");
        }
    }
}