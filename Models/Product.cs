namespace DotNet8Learning.Api.Models;

public class Product
{
    public int? Id { get; set; }

    public string? Name { get; set; } = string.Empty;

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public bool? IsAvailable { get; set; }
}