using DotNet8Learning.Api.Models;
using DotNet8Learning.Api.Services;
using DotNet8Learning.Api.Repositories;

namespace DotNet8Learning.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException(
                "Product price must be greater than zero.");
        }

        if (product.Quantity < 0)
        {
            throw new ArgumentException(
                "Product quantity cannot be negative.");
        }

        product.IsAvailable = product.Quantity > 0;

        return await _productRepository.CreateAsync(product);
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException(
                "Product price must be greater than zero.");
        }

        if (product.Quantity < 0)
        {
            throw new ArgumentException(
                "Product quantity cannot be negative.");
        }

        product.IsAvailable = product.Quantity > 0;

        return await _productRepository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}