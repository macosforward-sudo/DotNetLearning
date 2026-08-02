using DotNet8Learning.Api.Models;
using Microsoft.AspNetCore.Mvc;
using DotNet8Learning.Api.Repositories;
using DotNet8Learning.Api.Services;
namespace DotNet8Learning.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }   


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound($"Product with ID {id} was not found.");
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var createProduct = await _productService.CreateProductAsync(product);

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product updatedProduct)
    {
        var isUpdated = await _productService.UpdateProductAsync(id, updatedProduct);

        if (!isUpdated)
        {
            return NotFound($"Product with ID {id} was not found.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var isDeleted = await _productService.DeleteProductAsync(id);

        if (!isDeleted)
        {
            return NotFound($"Product with ID {id} was not found.");
        }

        return NoContent();
    }
}
