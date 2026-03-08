public class ContactMessages : BaseEntity
{
    public string FullName{get;set;}=null!;
    public string Email{get;set;}=null!;
    public string Phone{get;set;}=null!;
    public string? Message{get;set;}
    public bool IsRead{get;set;}=false;
}