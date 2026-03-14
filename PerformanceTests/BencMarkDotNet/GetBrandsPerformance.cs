using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace PerformanceDemo.Benchmarks;

[MemoryDiagnoser]
public class GetBrandsBenchmark
{
    private ApplicationDbContext _context = null!;
    // private BrandRepository _repo = null!;
    // private ILogger _logger = null!;
    private BrandRepository _repo = null!;
    // private IMemoryCache _cache = null!;
    // private IMapper _mapper = null!;

    [Params(100, 1_000, 10_000)]
    public int BrandCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"Brands-benchmark-{BrandCount}")
            .Options;

        _context = new ApplicationDbContext(options);
        _repo = new BrandRepository(_context);

        var Brands = Enumerable.Range(1, BrandCount)
            .Select(index => new Brand
            {
                Id = index,
                Name = $"Brand {index}",
                Slug = $"brand-{index}",
                LogoUrl = $"Logo for Brand {index}"
            });

        await _context.Brands.AddRangeAsync(Brands);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public Task<List<Brand>> GetBrands()
    {
        return _repo.GetBrandsAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.DisposeAsync();
    }
}