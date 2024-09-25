using System.Reflection.Metadata.Ecma335;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class ProductService : IProductService
{

    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return new ProductDto
        {
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task<ProductDto> AddProductAsync(AddProductDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Price = productDto.Price
        };
        await _productRepository.AddProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        product.Name = productDto.Name;
        product.Price = productDto.Price;

        await _productRepository.UpdateProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task<bool> DeleteProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product!=null)
        {
            await _productRepository.DeleteProductByIdAsync(id);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
    {
        var products = await _productRepository.GetAllProductsAsync();
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price

        });
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(string name)
    {
        var products = await _productRepository.GetAllProductsByNameAsync(name);

        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        });
    }
}