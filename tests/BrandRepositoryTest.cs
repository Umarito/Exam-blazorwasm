using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class BrandRepositoryTests
{
    [Fact]
    public async Task AddBrandTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(AddBrandTest));
        
        var Repository = new BrandRepository(context);
        var brand = new Brand
        {
            Id = 1,
            Name = "Apple",
            LogoUrl = "apple logo",
            Slug = "apple"
        };

        await Repository.AddAsync(brand);
        var s = await context.Brands.FindAsync(1);
        Assert.Equal("Apple", s.Name);
    }

    [Fact]
    public async Task GetBrandsTask()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(GetBrandsTask));
        context.Brands.AddRange(
            new Brand{Id=1, Name="Apple", LogoUrl = "ok", Slug = "apple"},
            new Brand{Id=2, Name="Samsung", LogoUrl = "ok", Slug = "samsung"},
            new Brand{Id=3, Name="LG", LogoUrl = "ok", Slug = "LG"}
        );
        await context.SaveChangesAsync();
        
        var Repository = new BrandRepository(context);
        var result = await Repository.GetBrandsAsync();
        var ints = new int[3]{1,2,3};
        var nums = result.Select(x=>x.Id);
        Assert.Equal(ints, nums.ToArray());

    }

    [Fact]
    public async Task UpdateBrandTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(UpdateBrandTest));
        var Repository = new BrandRepository(context);
        await context.Brands.AddAsync(new Brand{Id=1, Name="Apple", LogoUrl="apple logo", Slug="apple"});
        await context.SaveChangesAsync();

        var a = new Brand{Id=10,Name="Apple 2", LogoUrl="apple logo 2", Slug="apple-2"};
        await Repository.UpdateAsync(a);

        var v = await context.Brands.FindAsync(10);
        Assert.Equal("Apple 2", v.Name);
        Assert.Equal("apple logo 2", v.LogoUrl);
        Assert.Equal("apple-2", v.Slug);
    }

    [Fact]
    public async Task DeleteBrandTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(DeleteBrandTest));
        context.Brands.Add(new Brand{Id=1, Name="Apple", LogoUrl="apple logo", Slug="apple"});
        await context.SaveChangesAsync();

        var Repository = new BrandRepository(context);
        await Repository.DeleteAsync(1);

        var d = await context.Brands.FindAsync(1);
        Assert.Null(d);
    }
}
