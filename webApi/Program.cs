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

    if (!context.Products.Any())
    {
        var smartphones = context.Categories.First(c => c.Slug == "smartphones");
        var laptops = context.Categories.First(c => c.Slug == "laptops");
        var appliances = context.Categories.First(c => c.Slug == "home-appliances");
        var audio = context.Categories.First(c => c.Slug == "audio");
        
        var apple = context.Brands.First(b => b.Slug == "apple");
        var samsung = context.Brands.First(b => b.Slug == "samsung");
        var sony = context.Brands.First(b => b.Slug == "sony");
        var dell = context.Brands.First(b => b.Slug == "dell");
        var bose = context.Brands.First(b => b.Slug == "bose");

        context.Products.AddRange(
            new Product { Name = "iPhone 15 Pro Max", Description = "The ultimate iPhone with Titanium design.", Price = 1199, OldPrice = 1299, StockQuantity = 25, CategoryId = smartphones.Id, BrandId = apple.Id, ImageUrl = "https://images.unsplash.com/photo-1696446701796-da61225697cc?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Samsung Galaxy S24 Ultra", Description = "AI-powered smartphone with 200MP camera.", Price = 1299, OldPrice = 1399, StockQuantity = 30, CategoryId = smartphones.Id, BrandId = samsung.Id, ImageUrl = "https://images.unsplash.com/photo-1610945415295-d9bbf067e59c?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "MacBook Air M3", Description = "Thinner, lighter, and faster with M3 chip.", Price = 1099, OldPrice = 1199, StockQuantity = 15, CategoryId = laptops.Id, BrandId = apple.Id, ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Dell XPS 15", Description = "Stunning display and incredible performance.", Price = 1899, OldPrice = 2099, StockQuantity = 10, CategoryId = laptops.Id, BrandId = dell.Id, ImageUrl = "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "Sony WH-1000XM5", Description = "Industry-leading noise canceling headphones.", Price = 399, OldPrice = 449, StockQuantity = 40, CategoryId = audio.Id, BrandId = sony.Id, ImageUrl = "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Bose QuietComfort Ultra", Description = "World-class quiet and spatial audio.", Price = 429, OldPrice = 479, StockQuantity = 20, CategoryId = audio.Id, BrandId = bose.Id, ImageUrl = "https://images.unsplash.com/photo-1546435770-a3e426ff472b?auto=format&fit=crop&q=80&w=800", IsFeatured = false },
            new Product { Name = "Samsung Bespoke AI Fridge", Description = "Smart refrigerator with custom panels.", Price = 2499, OldPrice = 2799, StockQuantity = 5, CategoryId = appliances.Id, BrandId = samsung.Id, ImageUrl = "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?auto=format&fit=crop&q=80&w=800", IsFeatured = true },
            new Product { Name = "Sony PlayStation 5", Description = "The next generation of gaming.", Price = 499, OldPrice = 549, StockQuantity = 100, CategoryId = audio.Id, BrandId = sony.Id, ImageUrl = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?auto=format&fit=crop&q=80&w=800", IsFeatured = true }
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