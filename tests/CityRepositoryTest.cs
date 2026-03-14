using Microsoft.Extensions.Logging.Abstractions;

public class CityRepositoryTests
{
    [Fact]
    public async Task AddCityTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(AddCityTest));
        var repo = new CityRepository(context);
        var city = new City
        {
            Id = 1,
            Name = "Dushanbe"
        };

        await repo.AddAsync(city);

        var s = await context.Cities.FindAsync(1);
        Assert.Equal("Dushanbe", s.Name);
    }

    [Fact]
    public async Task GetCityByIdTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(GetCityByIdTest));
        context.Cities.Add(new City{Id=5, Name="Dushanbe"});
        await context.SaveChangesAsync();

        var repo = new CityRepository(context);
        var res = await repo.GetByIdAsync(5);

        Assert.Equal("Dushanbe", res.Name);
    }
}
