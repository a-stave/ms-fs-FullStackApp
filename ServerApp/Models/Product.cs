// Consider adding a .NET library to your project so you don't have two competing object definitions in your client and server.

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }

    public Category Category { get; set; } = new Category();
}