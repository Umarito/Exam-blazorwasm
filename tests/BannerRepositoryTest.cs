using Microsoft.Extensions.Logging.Abstractions;

public class BannerRepositoryTests
{
    [Fact]
    public async Task AddBannerTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(AddBannerTest));
        var repo = new BannerRepository(context);
        var banner = new Banner
        {
            Id = 1,
            ImageUrl = "image",
            TargetUrl = "target",
            DisplayOrder = 1
        };

        await repo.AddAsync(banner);

        var s = await context.Banners.FindAsync(1);
        Assert.Equal(1, s.DisplayOrder);
    }

    [Fact]
    public async Task GetBannerByIdTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(GetBannerByIdTest));
        context.Banners.Add(new Banner{Id=2, ImageUrl="image", TargetUrl = "target", DisplayOrder=3});
        await context.SaveChangesAsync();

        var repo = new BannerRepository(context);
        var res = await repo.GetByIdAsync(2);

        Assert.Equal(3, res.DisplayOrder);
    }
}
