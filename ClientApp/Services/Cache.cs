using Blazored.SessionStorage;
using System.Net.Http.Json;

public class ProductService
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly HttpClient _http;

    public ProductService(ISessionStorageService sessionStorage, HttpClient http)
    {
        _sessionStorage = sessionStorage;
        _http = http;
    }

    public async Task<Product[]?> GetProductsAsync(CancellationToken cancellationToken)
    {
        var cached = await _sessionStorage.GetItemAsync<Product[]>("products");

        if (cached != null)
        {
            Console.WriteLine("Session cache retrieved.");      // Helps debug cache issues
            return cached;
        }

        var products = await _http.GetFromJsonAsync<Product[]>("http://localhost:5184/api/productlist");

        if (products != null)
        {
            await _sessionStorage.SetItemAsync("products", products);
            Console.WriteLine("Products cached in session storage.");
        }

        return products;
    }
}
