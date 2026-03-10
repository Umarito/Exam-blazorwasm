using AutoMapper;
using System.Linq;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<ApplicationUser,UserGetDto>();
        CreateMap<UserInsertDto,ApplicationUser>();
        CreateMap<UserUpdateDto,ApplicationUser>();
        CreateMap<PageSMS,PageGetDto>();
        CreateMap<PageInsertDto,PageSMS>();
        CreateMap<PageUpdateDto,PageSMS>();
        CreateMap<ContactMessages,ContactMessageGetDto>();
        CreateMap<ContactMessageInsertDto,ContactMessages>();
        CreateMap<ContactMessageUpdateDto,ContactMessages>();
        CreateMap<Order,OrderGetDto>();
        CreateMap<OrderInsertDto,Order>();
        CreateMap<OrderUpdateDto,Order>();
        CreateMap<OrderItem,OrderItemGetDto>();
        CreateMap<OrderItemInsertDto,OrderItem>();
        CreateMap<OrderItemUpdateDto,OrderItem>();
        CreateMap<Cart,CartGetDto>();
        CreateMap<CartInsertDto,Cart>();
        CreateMap<CartUpdateDto,Cart>();
        CreateMap<Category,CategoryGetDto>()
            .ForMember(d => d.Products, o => o.Ignore());
        CreateMap<CategoryInsertDto,Category>();
        CreateMap<CategoryUpdateDto,Category>();
        CreateMap<CartItem,CartItemGetDto>();
        CreateMap<CartItemInsertDto,CartItem>();
        CreateMap<CartItemUpdateDto,CartItem>();
        CreateMap<Product,ProductGetDto>()
            .ForMember(d => d.OriginalPrice, o => o.MapFrom(s => s.OldPrice))
            .ForMember(d => d.AverageRating, o => o.MapFrom(s => s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : 0))
            .ForMember(d => d.ReviewCount, o => o.MapFrom(s => s.Reviews.Count));
        CreateMap<ProductInsertDto,Product>();
        CreateMap<ProductUpdateDto,Product>();
        CreateMap<ProductImage,ProductImageGetDto>();
        CreateMap<ProductImageInsertDto,ProductImage>();
        CreateMap<ProductImageUpdateDto,ProductImage>();
        CreateMap<ProductAttribute,ProductAttributeGetDto>();
        CreateMap<ProductAttributeInsertDto,ProductAttribute>();
        CreateMap<ProductAttributeUpdateDto,ProductAttribute>();
        CreateMap<Attribute,AttributeGetDto>();
        CreateMap<AttributeInsertDto,Attribute>();
        CreateMap<AttributeUpdateDto,Attribute>();
        CreateMap<Wishlist,WishlistGetDto>();
        CreateMap<WishlistInsertDto,Wishlist>();
        CreateMap<WishlistUpdateDto,Wishlist>();
        CreateMap<StoreLocation,StoreLocationGetDto>();
        CreateMap<StoreLocationInsertDto,StoreLocation>();
        CreateMap<StoreLocationUpdateDto,StoreLocation>();
        CreateMap<Installment,InstallmentGetDto>();
        CreateMap<InstallmentInsertDto,Installment>();
        CreateMap<InstallmentUpdateDto,Installment>();
        CreateMap<Brand,BrandGetDto>()
            .ForMember(d => d.Products, o => o.Ignore());
        CreateMap<BrandInsertDto,Brand>();
        CreateMap<BrandUpdateDto,Brand>();
        CreateMap<Banner,BannerGetDto>();
        CreateMap<BannerInsertDto,Banner>();
        CreateMap<BannerUpdateDto,Banner>();
        CreateMap<City,CityGetDto>();
        CreateMap<CityInsertDto,City>();
        CreateMap<CityUpdateDto,City>();
        CreateMap<Review,ReviewGetDto>();
        CreateMap<ReviewInsertDto,Review>();
        CreateMap<ReviewUpdateDto,Review>();
    }
}
