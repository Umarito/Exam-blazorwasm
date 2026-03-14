using System.Net.Http.Json;
using Admin.Models;

namespace Admin.Services;

public class AdminApiService
{
    private readonly HttpClient _http;

    public AdminApiService(HttpClient http)
    {
        _http = http;
    }

    #region Products
    public async Task<List<Product>> GetProductsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Product>>("products?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var response = await _http.GetFromJsonAsync<Response<Product>>($"products/{id}");
        return response?.Data;
    }

    public async Task<bool> CreateProductAsync(Product product)
    {
        var response = await _http.PostAsJsonAsync("products", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var response = await _http.PutAsJsonAsync($"products/{product.Id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _http.DeleteAsync($"products/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Categories
    public async Task<List<Category>> GetCategoriesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Category>>("categories?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        var response = await _http.PostAsJsonAsync("categories", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        var response = await _http.PutAsJsonAsync($"categories/{category.Id}", category);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var response = await _http.DeleteAsync($"categories/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Users
    public async Task<List<User>> GetUsersAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<User>>("users?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("users", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var response = await _http.PutAsJsonAsync($"users/{user.Id}", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"users/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Orders
    public async Task<List<Order>> GetOrdersAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Order>>("orders?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateOrderAsync(Order order)
    {
        var response = await _http.PostAsJsonAsync("orders", order);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        var response = await _http.PutAsJsonAsync($"orders/{order.Id}", order);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var response = await _http.DeleteAsync($"orders/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region OrderItems
    public async Task<List<OrderItem>> GetOrderItemsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<OrderItem>>("orderitems?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateOrderItemAsync(OrderItem item)
    {
        var response = await _http.PostAsJsonAsync("orderitems", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateOrderItemAsync(OrderItem item)
    {
        var response = await _http.PutAsJsonAsync($"orderitems/{item.Id}", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        var response = await _http.DeleteAsync($"orderitems/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Reviews
    public async Task<List<Review>> GetReviewsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Review>>("reviews?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateReviewAsync(Review review)
    {
        var response = await _http.PostAsJsonAsync("reviews", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReviewAsync(Review review)
    {
        var response = await _http.PutAsJsonAsync($"reviews/{review.Id}", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReviewAsync(int id)
    {
        var response = await _http.DeleteAsync($"reviews/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Banners
    public async Task<List<Banner>> GetBannersAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Banner>>("banners?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateBannerAsync(Banner banner)
    {
        var response = await _http.PostAsJsonAsync("banners", banner);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBannerAsync(Banner banner)
    {
        var response = await _http.PutAsJsonAsync($"banners/{banner.Id}", banner);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBannerAsync(int id)
    {
        var response = await _http.DeleteAsync($"banners/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Cities
    public async Task<List<City>> GetCitiesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<City>>("cities?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateCityAsync(City city)
    {
        var response = await _http.PostAsJsonAsync("cities", city);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCityAsync(City city)
    {
        var response = await _http.PutAsJsonAsync($"cities/{city.Id}", city);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCityAsync(int id)
    {
        var response = await _http.DeleteAsync($"cities/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region StoreLocations
    public async Task<List<StoreLocation>> GetStoreLocationsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<StoreLocation>>("storelocations?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateStoreLocationAsync(StoreLocation loc)
    {
        var response = await _http.PostAsJsonAsync("storelocations", loc);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateStoreLocationAsync(StoreLocation loc)
    {
        var response = await _http.PutAsJsonAsync($"storelocations/{loc.Id}", loc);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteStoreLocationAsync(int id)
    {
        var response = await _http.DeleteAsync($"storelocations/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Pages
    public async Task<List<Page>> GetPagesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Page>>("pages?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreatePageAsync(Page page)
    {
        var response = await _http.PostAsJsonAsync("pages", page);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePageAsync(Page page)
    {
        var response = await _http.PutAsJsonAsync($"pages/{page.Id}", page);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePageAsync(int id)
    {
        var response = await _http.DeleteAsync($"pages/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region ProductImages
    public async Task<List<ProductImage>> GetProductImagesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<ProductImage>>("productimages?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateProductImageAsync(ProductImage image)
    {
        var response = await _http.PostAsJsonAsync("productimages", image);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductImageAsync(ProductImage image)
    {
        var response = await _http.PutAsJsonAsync($"productimages/{image.Id}", image);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductImageAsync(int id)
    {
        var response = await _http.DeleteAsync($"productimages/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Attributes
    public async Task<List<AttributeModel>> GetAttributesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<AttributeModel>>("attributes?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateAttributeAsync(AttributeModel attr)
    {
        var response = await _http.PostAsJsonAsync("attributes", attr);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAttributeAsync(AttributeModel attr)
    {
        var response = await _http.PutAsJsonAsync($"attributes/{attr.Id}", attr);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAttributeAsync(int id)
    {
        var response = await _http.DeleteAsync($"attributes/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region ProductAttributes
    public async Task<List<ProductAttribute>> GetProductAttributesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<ProductAttribute>>("productattributes?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateProductAttributeAsync(ProductAttribute attr)
    {
        var response = await _http.PostAsJsonAsync("productattributes", attr);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAttributeAsync(ProductAttribute attr)
    {
        var response = await _http.PutAsJsonAsync($"productattributes/{attr.Id}", attr);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAttributeAsync(int id)
    {
        var response = await _http.DeleteAsync($"productattributes/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region ContactMessages
    public async Task<List<ContactMessage>> GetContactMessagesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<ContactMessage>>("contactmessages?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateContactMessageAsync(ContactMessage msg)
    {
        var response = await _http.PostAsJsonAsync("contactmessages", msg);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateContactMessageAsync(ContactMessage msg)
    {
        var response = await _http.PutAsJsonAsync($"contactmessages/{msg.Id}", msg);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteContactMessageAsync(int id)
    {
        var response = await _http.DeleteAsync($"contactmessages/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Carts
    public async Task<List<Cart>> GetCartsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Cart>>("carts?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateCartAsync(Cart cart)
    {
        var response = await _http.PostAsJsonAsync("carts", cart);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCartAsync(Cart cart)
    {
        var response = await _http.PutAsJsonAsync($"carts/{cart.Id}", cart);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCartAsync(int id)
    {
        var response = await _http.DeleteAsync($"carts/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region CartItems
    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<CartItem>>("cartitems?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateCartItemAsync(CartItem item)
    {
        var response = await _http.PostAsJsonAsync("cartitems", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCartItemAsync(CartItem item)
    {
        var response = await _http.PutAsJsonAsync($"cartitems/{item.Id}", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCartItemAsync(int id)
    {
        var response = await _http.DeleteAsync($"cartitems/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Wishlists
    public async Task<List<Wishlist>> GetWishlistsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Wishlist>>("wishlists?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateWishlistAsync(Wishlist item)
    {
        var response = await _http.PostAsJsonAsync("wishlists", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateWishlistAsync(Wishlist item)
    {
        var response = await _http.PutAsJsonAsync($"wishlists/{item.Id}", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteWishlistAsync(int id)
    {
        var response = await _http.DeleteAsync($"wishlists/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Installments
    public async Task<List<Installment>> GetInstallmentsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Installment>>("installments?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateInstallmentAsync(Installment installment)
    {
        var response = await _http.PostAsJsonAsync("installments", installment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteInstallmentAsync(int id)
    {
        var response = await _http.DeleteAsync($"installments/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Brands
    public async Task<List<Brand>> GetBrandsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Brand>>("brands?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateBrandAsync(Brand brand)
    {
        var response = await _http.PostAsJsonAsync("brands", brand);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBrandAsync(Brand brand)
    {
        var response = await _http.PutAsJsonAsync($"brands/{brand.Id}", brand);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBrandAsync(int id)
    {
        var response = await _http.DeleteAsync($"brands/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion
}
