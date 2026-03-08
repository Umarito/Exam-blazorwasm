using WebApi.DTOs;

public interface IContactMessageService
{
    Task<Response<string>> AddContactMessageAsync(ContactMessageInsertDto ContactMessageInsertDto);
    Task<Response<ContactMessageGetDto>> GetContactMessageByIdAsync(int ContactMessageId);
    Task<Response<List<ContactMessageGetDto>>> GetAllContactMessagesAsync();
    Task<Response<string>> DeleteAsync(int ContactMessageId);
    Task<Response<string>> UpdateAsync(int ContactMessageId,ContactMessageUpdateDto ContactMessageUpdateDto);
}