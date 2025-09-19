using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow outside traffic to access our API. A more restrictive origin policy might be worthwhile for internal APIs?
builder.Services.AddCors();

// Add server-side caching to improve performance for commonly-requested data. ('course, we only have the one data, but I'm an eternal optimist!)
builder.Services.AddMemoryCache();

var app = builder.Build();

// AllowAnyOrigin is alright for development, but I'd probably want to reconfigure it for production.
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// GET returns the "product list," which is cached for 5 minutes to improve performance.
app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    if (!cache.TryGetValue("productlist", out Product[]? products))
    {
        products = new[]
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25, Category = new Category { Id = 101, Name = "Electronics" }},
            new Product { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100, Category = new Category { Id = 102, Name = "Accessories" }}
        };

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        cache.Set("productlist", products, options);
    }

    return Results.Json(products);
});

app.Run();