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
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE,
            ManufacturerProducts = product.Manufacturers
            
        };
    }

    public async Task<ProductDto> AddProductAsync(AddUpdateProductDto updateProductDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = updateProductDto.Name,
            Price = updateProductDto.Price,
            Status = updateProductDto.DOE > updateProductDto.DOP,
            Inventory = updateProductDto.Inventory,
            DOP = updateProductDto.DOP,
            DOE = updateProductDto.DOE,
            Manufacturers = []
        };
        await _productRepository.AddProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE,
            ManufacturerProducts = product.Manufacturers
        };
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, AddUpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Status = updateProductDto.Status;
        product.Inventory = updateProductDto.Inventory;
        product.DOE = updateProductDto.DOE;
        product.DOP = updateProductDto.DOP;
        
        await _productRepository.UpdateProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE,
            ManufacturerProducts = product.Manufacturers
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
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE

        });
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(string name)
    {
        var products = await _productRepository.GetAllProductsByNameAsync(name);

        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE,
        });
    }

    public async Task<ProductDto> GetProductManufacturersAsync(Guid productId)
    {
        var product = await _productRepository.GetProductManufacturersAsync(productId);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.DOP,
            DOE = product.DOE,
            ManufacturerProducts = product.Manufacturers
        };
    }
}