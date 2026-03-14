using Microsoft.Extensions.Logging.Abstractions;

public class CategoryRepositoryTests
{
    [Fact]
    public async Task AddCategoryTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(AddCategoryTest));
        var repo = new CategoryRepository(context);
        var category = new Category
        {
            Id = 1,
            Name = "Phones",
            Slug = "phones",
            Description = "phones",
            IsActive = true
        };

        await repo.AddAsync(category);

        var s = await context.Categories.FindAsync(1);
        Assert.Equal("Phones", s.Name);
    }

    [Fact]
    public async Task GetCategoryByIdTest()
    {
        await using var context = TestAppDbContextFactory.CreateContext(nameof(GetCategoryByIdTest));
        context.Categories.Add(new Category{Id=7, Name="TV", Slug="tv"});
        await context.SaveChangesAsync();

        var repo = new CategoryRepository(context);
        var res = await repo.GetByIdAsync(7);

        Assert.Equal("TV", res.Name);
    }
}
