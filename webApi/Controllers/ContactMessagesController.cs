using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ContactMessagesController(IContactMessageService ContactMessageService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<string>> AddContactMessageAsync(ContactMessageInsertDto ContactMessage)
    {
        return await ContactMessageService.AddContactMessageAsync(ContactMessage);
    }
    [HttpPut("{ContactMessageId}")]
    public async Task<Response<string>> UpdateAsync(int ContactMessageId,ContactMessageUpdateDto ContactMessage)
    {
        return await ContactMessageService.UpdateAsync(ContactMessageId,ContactMessage);
    }
    [HttpDelete("{ContactMessageId}")]
    public async Task<Response<string>> DeleteAsync(int ContactMessageId)
    {
        return await ContactMessageService.DeleteAsync(ContactMessageId);
    }
    [HttpGet]
    public async Task<Response<List<ContactMessageGetDto>>> GetAllContactMessages()
    {
        return await ContactMessageService.GetAllContactMessagesAsync();   
    }
    
    [HttpGet("{ContactMessageId}")]
    public async Task<Response<ContactMessageGetDto>> GetContactMessageByIdAsync(int ContactMessageId)
    {
        return await ContactMessageService.GetContactMessageByIdAsync(ContactMessageId);
    }
}