using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using WebApi.EmailService;
using WebApi.Entities;
using WebApi.Seeds;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// builder.Services.AddScoped<IProductService, ProductService>();  
builder.Services.AddScoped<IJwtTokenService,JwtTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IPageRepository, PageSMSRepository>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessagesRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IStoreLocationService, StoreLocationService>();
builder.Services.AddScoped<IStoreLocationRepository, StoreLocationRepository>();
builder.Services.AddScoped<IInstallmentService, InstallmentService>();
builder.Services.AddScoped<IInstallmentRepository, InstallmentRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
builder.Services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
builder.Services.AddScoped<IAttributeService, AttributeService>();
builder.Services.AddScoped<IAttributeRepository, AttributeRepository>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();

// builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(InfrastructureProfile));
builder.Services.AddLogging(config => {config.AddConsole(); config.SetMinimumLevel(LogLevel.Information);});

var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddSwaggerGen(options=>{
     options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен так: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

// builder.Services.AddQuartz(q =>
// {
//     var jobKey = new JobKey("ReportJob");

//     q.AddJob<ReportJob>(opts => opts.WithIdentity(jobKey));

//     q.AddTrigger(opts => opts
//         .ForJob(jobKey)
//         .WithSimpleSchedule(x =>
//             x.WithIntervalInMinutes(1)
//              .RepeatForever()));
// });

// builder.Services.AddQuartzHostedService();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build(); 

try
{
    await using var scope = app.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedRoles(roleManager);

    // Seed data
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "Smartphones", Slug = "smartphones", Description = "Latest mobile devices" },
            new Category { Name = "Laptops", Slug = "laptops", Description = "Powerful portable computers" },
            new Category { Name = "Home Appliances", Slug = "home-appliances", Description = "Smart solutions for your home" },
            new Category { Name = "Audio", Slug = "audio", Description = "Premium sound equipment" },
            new Category { Name = "Wearables", Slug = "wearables", Description = "Smart watches and trackers" }
        );
        await context.SaveChangesAsync();
    }

    var categoryBySlug = context.Categories.ToDictionary(c => c.Slug, c => c);
    var subCategorySeeds = new[]
    {
        new { Name = "Android Phones", Slug = "android-phones", Description = "Top Android devices", Parent = "smartphones" },
        new { Name = "iPhone", Slug = "iphone", Description = "Apple iPhone lineup", Parent = "smartphones" },
        new { Name = "Ultrabooks", Slug = "ultrabooks", Description = "Thin and light laptops", Parent = "laptops" },
        new { Name = "Gaming Laptops", Slug = "gaming-laptops", Description = "High performance for gaming", Parent = "laptops" },
        new { Name = "Kitchen", Slug = "kitchen", Description = "Smart kitchen appliances", Parent = "home-appliances" },
        new { Name = "Cleaning", Slug = "cleaning", Description = "Vacuum and cleaning tech", Parent = "home-appliances" },
        new { Name = "Headphones", Slug = "headphones", Description = "Headphones and earbuds", Parent = "audio" },
        new { Name = "Speakers", Slug = "speakers", Description = "Wireless speakers and soundbars", Parent = "audio" },
        new { Name = "Smart Watches", Slug = "smart-watches", Description = "Wearable smart tech", Parent = "wearables" }
    };

    foreach (var sub in subCategorySeeds)
    {
        if (!context.Categories.Any(c => c.Slug == sub.Slug) && categoryBySlug.TryGetValue(sub.Parent, out var parent))
        {
            context.Categories.Add(new Category
            {
                Name = sub.Name,
                Slug = sub.Slug,
                Description = sub.Description,
                ParentCategoryId = parent.Id
            });
        }
    }
    await context.SaveChangesAsync();

    if (!context.Brands.Any())
    {
        context.Brands.AddRange(
            new Brand { Name = "Apple", Slug = "apple" },
            new Brand { Name = "Samsung", Slug = "samsung" },
            new Brand { Name = "Sony", Slug = "sony" },
            new Brand { Name = "Dell", Slug = "dell" },
            new Brand { Name = "Bose", Slug = "bose" }
        );
        await context.SaveChangesAsync();
    }

    var brandSeeds = new[]
    {
        new Brand { Name = "LG", Slug = "lg" },
        new Brand { Name = "HP", Slug = "hp" },
        new Brand { Name = "Lenovo", Slug = "lenovo" },
        new Brand { Name = "Xiaomi", Slug = "xiaomi" },
        new Brand { Name = "Microsoft", Slug = "microsoft" }
    };
    foreach (var brand in brandSeeds)
    {
        if (!context.Brands.Any(b => b.Slug == brand.Slug))
            context.Brands.Add(brand);
    }
    await context.SaveChangesAsync();

    if (!context.Products.Any())
    {
        var smartphones = context.Categories.First(c => c.Slug == "smartphones");
        var laptops = context.Categories.First(c => c.Slug == "laptops");
        var appliances = context.Categories.First(c => c.Slug == "home-appliances");
        var audio = context.Categories.First(c => c.Slug == "audio");
        var android = context.Categories.FirstOrDefault(c => c.Slug == "android-phones") ?? smartphones;
        var iphone = context.Categories.FirstOrDefault(c => c.Slug == "iphone") ?? smartphones;
        var ultrabooks = context.Categories.FirstOrDefault(c => c.Slug == "ultrabooks") ?? laptops;
        var gamingLaptops = context.Categories.FirstOrDefault(c => c.Slug == "gaming-laptops") ?? laptops;
        var headphones = context.Categories.FirstOrDefault(c => c.Slug == "headphones") ?? audio;
        var speakers = context.Categories.FirstOrDefault(c => c.Slug == "speakers") ?? audio;
        
        var apple = context.Brands.First(b => b.Slug == "apple");
        var samsung = context.Brands.First(b => b.Slug == "samsung");
        var sony = context.Brands.First(b => b.Slug == "sony");
        var dell = context.Brands.First(b => b.Slug == "dell");
        var bose = context.Brands.First(b => b.Slug == "bose");
        var lg = context.Brands.First(b => b.Slug == "lg");
        var hp = context.Brands.First(b => b.Slug == "hp");
        var lenovo = context.Brands.First(b => b.Slug == "lenovo");
        var xiaomi = context.Brands.First(b => b.Slug == "xiaomi");
        var microsoft = context.Brands.First(b => b.Slug == "microsoft");

        context.Products.AddRange(
            new Product { Name = "iPhone 15 Pro Max", Description = "The ultimate iPhone with Titanium design.", Price = 1199, OldPrice = 1299, StockQuantity = 25, CategoryId = iphone.Id, BrandId = apple.Id, ImageUrl = "https://images.unsplash.com/photo-1696446701796-da61225697cc?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Samsung Galaxy S24 Ultra", Description = "AI-powered smartphone with 200MP camera.", Price = 1299, OldPrice = 1399, StockQuantity = 30, CategoryId = android.Id, BrandId = samsung.Id, ImageUrl = "https://images.unsplash.com/photo-1610945415295-d9bbf067e59c?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Xiaomi 14 Pro", Description = "Flagship performance with Leica camera.", Price = 899, OldPrice = 999, StockQuantity = 42, CategoryId = android.Id, BrandId = xiaomi.Id, ImageUrl = "https://images.unsplash.com/photo-1523206489230-c012c64b2b48?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "MacBook Air M3", Description = "Thinner, lighter, and faster with M3 chip.", Price = 1099, OldPrice = 1199, StockQuantity = 15, CategoryId = ultrabooks.Id, BrandId = apple.Id, ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Dell XPS 15", Description = "Stunning display and incredible performance.", Price = 1899, OldPrice = 2099, StockQuantity = 10, CategoryId = ultrabooks.Id, BrandId = dell.Id, ImageUrl = "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "HP Spectre x360", Description = "Convertible ultrabook with premium design.", Price = 1599, OldPrice = 1799, StockQuantity = 18, CategoryId = ultrabooks.Id, BrandId = hp.Id, ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "Lenovo Legion 7", Description = "High-performance gaming laptop.", Price = 2099, OldPrice = 2299, StockQuantity = 7, CategoryId = gamingLaptops.Id, BrandId = lenovo.Id, ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Sony WH-1000XM5", Description = "Industry-leading noise canceling headphones.", Price = 399, OldPrice = 449, StockQuantity = 40, CategoryId = headphones.Id, BrandId = sony.Id, ImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Bose QuietComfort Ultra", Description = "World-class quiet and spatial audio.", Price = 429, OldPrice = 479, StockQuantity = 20, CategoryId = headphones.Id, BrandId = bose.Id, ImageUrl = "https://images.unsplash.com/photo-1546435770-a3e426ff472b?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "LG Soundbar S90QY", Description = "Cinema-grade Dolby Atmos soundbar.", Price = 699, OldPrice = 799, StockQuantity = 12, CategoryId = speakers.Id, BrandId = lg.Id, ImageUrl = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "Samsung Bespoke AI Fridge", Description = "Smart refrigerator with custom panels.", Price = 2499, OldPrice = 2799, StockQuantity = 5, CategoryId = appliances.Id, BrandId = samsung.Id, ImageUrl = "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Sony PlayStation 5", Description = "The next generation of gaming.", Price = 499, OldPrice = 549, StockQuantity = 100, CategoryId = audio.Id, BrandId = sony.Id, ImageUrl = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Microsoft Surface Pro 9", Description = "Versatile 2-in-1 tablet with performance.", Price = 1299, OldPrice = 1399, StockQuantity = 22, CategoryId = ultrabooks.Id, BrandId = microsoft.Id, ImageUrl = "https://images.unsplash.com/photo-1519389950473-47ba0277781c?auto=format&fit=crop&q=80&w=800", IsFeatured = false }
        );
        await context.SaveChangesAsync();
    }

    if (!context.Attributes.Any())
    {
        context.Attributes.AddRange(
            new Attribute { Name = "Storage" },
            new Attribute { Name = "RAM" },
            new Attribute { Name = "Color" },
            new Attribute { Name = "Screen Size" },
            new Attribute { Name = "Battery" }
        );
        await context.SaveChangesAsync();
    }

    if (!context.ProductAttributes.Any())
    {
        var storage = context.Attributes.First(a => a.Name == "Storage");
        var ram = context.Attributes.First(a => a.Name == "RAM");
        var color = context.Attributes.First(a => a.Name == "Color");
        var screen = context.Attributes.First(a => a.Name == "Screen Size");
        var battery = context.Attributes.First(a => a.Name == "Battery");

        foreach (var product in context.Products.ToList())
        {
            context.ProductAttributes.AddRange(
                new ProductAttribute { ProductId = product.Id, AttributeId = storage.Id, Value = "256 GB" },
                new ProductAttribute { ProductId = product.Id, AttributeId = ram.Id, Value = "8 GB" },
                new ProductAttribute { ProductId = product.Id, AttributeId = color.Id, Value = "Black" },
                new ProductAttribute { ProductId = product.Id, AttributeId = screen.Id, Value = "6.5 in" },
                new ProductAttribute { ProductId = product.Id, AttributeId = battery.Id, Value = "4500 mAh" }
            );
        }
        await context.SaveChangesAsync();
    }

    if (!context.Installments.Any())
    {
        context.Installments.AddRange(
            new Installment { MonthCount = 3, InterestRate = 0 },
            new Installment { MonthCount = 6, InterestRate = 2.5m },
            new Installment { MonthCount = 12, InterestRate = 4.5m },
            new Installment { MonthCount = 24, InterestRate = 5.5m }
        );
        await context.SaveChangesAsync();
    }
    app.Logger.LogInformation("Finished Seeding Enhanced Data");
}
catch (Exception ex)
{
    app.Logger.LogError("An error occurred while seeding the db: {ExMessage}", ex.Message);
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

    var adminEmail = "admin@gmail.com";

    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin User"
        };

        await userManager.CreateAsync(newAdmin, "Admin123");
        await userManager.AddToRoleAsync(newAdmin, "Admin");
    }

    // Create Guest Account for demo
    var guestEmail = "guest@avrang.tj";
    var guest = await userManager.FindByEmailAsync(guestEmail);
    if (guest == null)
    {
        var newGuest = new ApplicationUser
        {
            UserName = guestEmail,
            Email = guestEmail,
            FullName = "Guest User",
            IsActive = true
        };
        await userManager.CreateAsync(newGuest, "Guest123!");
        await userManager.AddToRoleAsync(newGuest, "User");
    }

    var demoUsers = new[]
    {
        new { Email = "user1@avrang.tj", Name = "Ali Karimov" },
        new { Email = "user2@avrang.tj", Name = "Zarina Rustamova" },
        new { Email = "user3@avrang.tj", Name = "Said Niyazov" }
    };

    foreach (var u in demoUsers)
    {
        var existing = await userManager.FindByEmailAsync(u.Email);
        if (existing == null)
        {
            var newUser = new ApplicationUser
            {
                UserName = u.Email,
                Email = u.Email,
                FullName = u.Name,
                IsActive = true
            };
            await userManager.CreateAsync(newUser, "User123!");
            await userManager.AddToRoleAsync(newUser, "User");
        }
    }

    if (!db.Reviews.Any())
    {
        var users = db.Users.Take(5).ToList();
        var products = db.Products.ToList();
        var rnd = new Random(2026);

        foreach (var product in products)
        {
            var reviewCount = rnd.Next(2, 5);
            for (var i = 0; i < reviewCount; i++)
            {
                var reviewer = users[rnd.Next(users.Count)];
                db.Reviews.Add(new Review
                {
                    ProductId = product.Id,
                    UserId = reviewer.Id,
                    Rating = rnd.Next(3, 6),
                    Comment = "Great quality and fast delivery.",
                    IsApproved = true
                });
            }
        }
        await db.SaveChangesAsync();
    }

    // // Seed data
    // var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // if (!context.Categories.Any())
    // {
    //     context.Categories.Add(new Category { Name = "Electronics", Slug = "electronics", Description = "Electronic devices" });
    //     context.Categories.Add(new Category { Name = "Clothing", Slug = "clothing", Description = "Clothing items" });
    //     await context.SaveChangesAsync();
    // }

    // if (!context.Brands.Any())
    // {
    //     context.Brands.Add(new Brand { Name = "Apple", Slug = "apple" });
    //     context.Brands.Add(new Brand { Name = "Nike", Slug = "nike" });
    //     await context.SaveChangesAsync();
    // }

    // if (!context.Products.Any())
    // {
    //     var electronics = context.Categories.First(c => c.Name == "Electronics");
    //     var apple = context.Brands.First(b => b.Name == "Apple");

    //     context.Products.Add(new Product
    //     {
    //         Name = "iPhone 15",
    //         Description = "Latest iPhone",
    //         Price = 999,
    //         OldPrice = 1099,
    //         StockQuantity = 10,
    //         CategoryId = electronics.Id,
    //         BrandId = apple.Id
    //     });
    //     await context.SaveChangesAsync();
    // }
}


app.UseMiddleware<RequestTimeMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
