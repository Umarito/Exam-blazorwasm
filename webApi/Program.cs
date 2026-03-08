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
            new Category { Name = "Electronics", Slug = "electronics", Description = "Electronic devices and gadgets" },
            new Category { Name = "Clothing", Slug = "clothing", Description = "Fashion and apparel" },
            new Category { Name = "Home & Garden", Slug = "home-garden", Description = "Home improvement and garden supplies" }
        );
        await context.SaveChangesAsync();
        app.Logger.LogInformation("Seeded categories");
    }

    if (!context.Brands.Any())
    {
        context.Brands.AddRange(
            new Brand { Name = "Apple", Slug = "apple" },
            new Brand { Name = "Samsung", Slug = "samsung" },
            new Brand { Name = "Nike", Slug = "nike" },
            new Brand { Name = "Adidas", Slug = "adidas" }
        );
        await context.SaveChangesAsync();
        app.Logger.LogInformation("Seeded brands");
    }

    if (!context.Products.Any())
    {
        var electronics = context.Categories.First(c => c.Name == "Electronics");
        var clothing = context.Categories.First(c => c.Name == "Clothing");
        var apple = context.Brands.First(b => b.Name == "Apple");
        var samsung = context.Brands.First(b => b.Name == "Samsung");
        var nike = context.Brands.First(b => b.Name == "Nike");

        context.Products.AddRange(
            new Product
            {
                Name = "iPhone 15 Pro",
                Description = "Latest iPhone with advanced features",
                Price = 999,
                OldPrice = 1099,
                StockQuantity = 50,
                CategoryId = electronics.Id,
                BrandId = apple.Id
            },
            new Product
            {
                Name = "Samsung Galaxy S24",
                Description = "Powerful Android smartphone",
                Price = 799,
                OldPrice = 899,
                StockQuantity = 30,
                CategoryId = electronics.Id,
                BrandId = samsung.Id
            },
            new Product
            {
                Name = "Nike Air Max",
                Description = "Comfortable running shoes",
                Price = 120,
                OldPrice = 150,
                StockQuantity = 100,
                CategoryId = clothing.Id,
                BrandId = nike.Id
            },
            new Product
            {
                Name = "iPad Air",
                Description = "Versatile tablet for work and play",
                Price = 599,
                OldPrice = 649,
                StockQuantity = 20,
                CategoryId = electronics.Id,
                BrandId = apple.Id
            },
            new Product
            {
                Name = "MacBook Pro 14\"",
                Description = "High-performance laptop",
                Price = 1999,
                OldPrice = 2199,
                StockQuantity = 10,
                CategoryId = electronics.Id,
                BrandId = apple.Id
            }
        );
        await context.SaveChangesAsync();
        app.Logger.LogInformation("Seeded products");
    }

    app.Logger.LogInformation("Finished Seeding Default Data");
    app.Logger.LogInformation("Application Starting");
}
catch (Exception ex)
{
    app.Logger.LogError("An Error occurred while seeding the db:  {ExMessage}", ex.Message);
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