public class BannerGetDto
{
    public int Id{get;set;}
    public string ImageUrl{get;set;}=null!;
    public string? TargetUrl{get;set;}
    public int DisplayOrder{get;set;}
}