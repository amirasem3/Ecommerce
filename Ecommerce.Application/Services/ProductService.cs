using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    public const string ProductException = "Product Not Found!";

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            throw new Exception(ProductException);
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Inventory = product.Inventory,
            Status = product.Status,
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = product.Manufacturers2.Select(m => new ManufacturerProductDto
            {
                Address = m.Address,
                Email = m.Email,
                Country = m.ManufacturerCountry,
                Name = m.Name,
                PhoneNumber = m.PhoneNumber,
                Rate = m.Rate
            }).ToList()
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
            Manufacturers2 = []
        };
        await _productRepository.AddProductAsync(product);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = []
        };
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, AddUpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            throw new Exception(ProductException);
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
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = product.Manufacturers2.Select(m => new ManufacturerProductDto
            {
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                Country = m.ManufacturerCountry,
                Email = m.Email,
                Name = m.Name,
                Rate = m.Rate
            }).ToList()
        };
    }

    public async Task<bool> DeleteProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            throw new Exception(ProductException);
        }

        await _productRepository.DeleteProductByIdAsync(id);
        return true;
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
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = product.Manufacturers2.Select(m => new ManufacturerProductDto
            {
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                Country = m.ManufacturerCountry,
                Email = m.Email,
                Name = m.Name,
                Rate = m.Rate
            }).ToList()
        }).ToList();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(string name)
    {
        var products = await _productRepository.GetAllProductsByNameAsync(name);
        if (products == null)
        {
            throw new Exception(ProductException);
        }

        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = product.Manufacturers2.Select(m => new ManufacturerProductDto
            {
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                Country = m.ManufacturerCountry,
                Email = m.Email,
                Name = m.Name,
                Rate = m.Rate
            }).ToList()
        }).ToList();
    }

    public async Task<IEnumerable<ProductDto>> FilterProductByPriceAsync(decimal startPrice, decimal endPrice)
    {
        var products = await _productRepository.FilterProductsByPrice(startPrice, endPrice);
        if (products == null)
        {
            throw new Exception(ProductException);
        }
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Status = product.Status,
            Inventory = product.Inventory,
            Dop = product.Dop,
            Doe = product.Doe,
            Manufacturer = product.Manufacturers2.Select(m => new ManufacturerProductDto
            {
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                Country = m.ManufacturerCountry,
                Email = m.Email,
                Name = m.Name,
                Rate = m.Rate
            }).ToList()
        }).ToList();
    }

    public async Task<IEnumerable<InvoiceProductDto>> GetInvoicesByProductId(Guid productId)
    {
        var invoices = await _productRepository.GetInvoicesByProductIdAsync(productId);
        if (invoices==null)
        {
            throw new Exception("No Invoice Found!");
        }

        return invoices.Select(invoice => new InvoiceProductDto()
        {
            Id = invoice.Id,
            IssueDate = invoice.IssueDate,
            OwnerName = invoice.OwnerFirstName,
            OwnerFamilyName = invoice.OwnerLastName,
            IdentificationCode = invoice.IdentificationCode,
            IssuerName = invoice.IssuerName,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice,
        });
    }
}