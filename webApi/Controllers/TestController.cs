using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

    [HttpGet("products")]
    public IActionResult GetTestProducts()
    {
        var result = new
        {
            items = new[]
            {
                new { id = 1, name = "iPhone 15 Pro", price = 999, description = "Latest iPhone", stockQuantity = 50 },
                new { id = 2, name = "Samsung Galaxy S24", price = 799, description = "Powerful Android smartphone", stockQuantity = 30 },
                new { id = 3, name = "iPad Air", price = 599, description = "Versatile tablet", stockQuantity = 20 }
            },
            page = 1,
            pageSize = 10,
            totalCount = 3,
            totalPages = 1,
            hasNext = false,
            hasPrevious = false
        };
        return Ok(result);
    }
}
