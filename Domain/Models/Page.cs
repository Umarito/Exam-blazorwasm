public class PageSMS : BaseEntity
{
    public string Title{get;set;}=null!;
    public string Slug{get;set;}=null!;
    public string Content{get;set;}=null!;
    public bool IsPublished{get;set;}=false;
}