using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class FakeDataService
{
    public List<Category> Categories { get; private set; }
    public List<Brand> Brands { get; private set; }
    public List<Product> Products { get; private set; }

    public FakeDataService()
    {
        Categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Home Appliances" },
            new Category { Id = 3, Name = "Audio & Headphones" },
            new Category { Id = 4, Name = "Photography" }
        };

        Brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Sony" },
            new Brand { Id = 2, Name = "Apple" },
            new Brand { Id = 3, Name = "Samsung" }
        };

        Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Premium Noise-Cancelling Headphones",
                Description = "Experience industry-leading noise cancellation and premium sound quality. Designed for all-day comfort with up to 30 hours of battery life.",
                Price = 299m,
                OriginalPrice = 349m,
                CategoryId = 3,
                BrandId = 1,
                AverageRating = 4.8,
                ReviewCount = 128,
                IsFeatured = true,
                IsTrending = false,
                ImageUrl = ""
            },
            new Product
            {
                Id = 2,
                Name = "Smart Watch Pro Series",
                Description = "Advanced smartwatch.",
                Price = 199m,
                OriginalPrice = 199m,
                CategoryId = 1,
                BrandId = 2,
                AverageRating = 4.8,
                ReviewCount = 55,
                IsFeatured = false,
                IsTrending = true,
                ImageUrl = ""
            },
            new Product
            {
                Id = 3,
                Name = "Minimalist Coffee Maker",
                Description = "Brew perfectly.",
                Price = 149.50m,
                OriginalPrice = 149.50m,
                CategoryId = 2,
                BrandId = 3,
                AverageRating = 4.5,
                ReviewCount = 30,
                IsFeatured = false,
                IsTrending = true,
                ImageUrl = ""
            },
            new Product
            {
                Id = 4,
                Name = "Wireless Earbuds X",
                Description = "In-ear sound.",
                Price = 89m,
                OriginalPrice = 110m,
                CategoryId = 3,
                BrandId = 1,
                AverageRating = 4.9,
                ReviewCount = 150,
                IsFeatured = false,
                IsTrending = true,
                ImageUrl = ""
            },
            new Product
            {
                Id = 5,
                Name = "Compact Digital Camera",
                Description = "Capture the moment.",
                Price = 450m,
                OriginalPrice = 500m,
                CategoryId = 4,
                BrandId = 1,
                AverageRating = 4.6,
                ReviewCount = 80,
                IsFeatured = false,
                IsTrending = true,
                ImageUrl = ""
            }
        };

        foreach (var p in Products)
        {
            p.Category = Categories.FirstOrDefault(c => c.Id == p.CategoryId);
            p.Brand = Brands.FirstOrDefault(b => b.Id == p.BrandId);
        }
    }
}
