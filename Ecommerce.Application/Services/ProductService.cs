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
            Inventory = product.Inventory,
            Status = product.Status,
            DOP = product.Dop,
            DOE = product.Doe,
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
            Inventory = updateProductDto.Inventory,
            Dop = updateProductDto.Dop,
            Doe = updateProductDto.Doe,
            Status = updateProductDto.Doe > updateProductDto.Dop && updateProductDto.Inventory > 0,
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
            DOP = product.Dop,
            DOE = product.Doe,
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
        product.Doe = updateProductDto.Doe;
        product.Dop = updateProductDto.Dop;

        await _productRepository.UpdateProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.Dop,
            DOE = product.Doe,
            ManufacturerProducts = product.Manufacturers
        };
    }

    public async Task<bool> DeleteProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product != null)
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
            DOP = product.Dop,
            DOE = product.Doe
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
            DOP = product.Dop,
            DOE = product.Doe,
        });
    }

    public async Task<IEnumerable<ProductDto>> FilterProductByPriceAsync(decimal startPrice, decimal endPrice)
    {
        var products = await _productRepository.FilterProductsByPrice(startPrice, endPrice);
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.Dop,
            DOE = product.Doe,
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
            DOP = product.Dop,
            DOE = product.Doe,
            ManufacturerProducts = product.Manufacturers
        };
    }

    public async Task<ProductDto> GetProductInvoicesAsync(Guid productId)
    {
        var product = await _productRepository.GetProductInvoicesAsync(productId);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            DOP = product.Dop,
            DOE = product.Doe,
            ManufacturerProducts = product.Manufacturers,
            ProductInvoices = product.Invoices
        };
    }
}