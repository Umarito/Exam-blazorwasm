public class UserGetDto
{
    public int Id{get;set;}
    public string FullName{get;set;}=null!;
    public string Email{get;set;}=null!;
    public string PhoneNumber{get;set;}=null!;
    public UserRole Role{get;set;}=UserRole.User;
    public CartGetDto? Cart{get;set;}
    public List<OrderGetDto>? Orders{get;set;}
}