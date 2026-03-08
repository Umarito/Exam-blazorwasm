public class Banner : BaseEntity
{
    public string ImageUrl{get;set;} = null!;
    public string? TargetUrl{get;set;} 
    public int DisplayOrder{get;set;}
}