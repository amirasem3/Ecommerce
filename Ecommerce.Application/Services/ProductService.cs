using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Repositories;

namespace Ecommerce.Application.Services;

public class ProductService
{
    public const string ProductException = "Product Not Found!";
    private readonly UnitOfWork _unitOfWork;

    public ProductService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
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
        await _unitOfWork.ProductRepository.InsertAsync(product);
        await _unitOfWork.SaveAsync();
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
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
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

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.SaveAsync();
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
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            throw new Exception(ProductException);
        }

        // await _productRepository.DeleteProductByIdAsync(id);
        await _unitOfWork.ProductRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetAsync(includeProperties: "Manufacturers2");
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
        var products = await _unitOfWork.ProductRepository.GetAsync(filter: p => p.Name.Contains(name),
            includeProperties: "Manufacturers2");
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
        var products = await _unitOfWork.ProductRepository.GetAsync(
            filter: p => p.Price >= startPrice && p.Price <= endPrice, includeProperties: "Manufacturers2");
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
        var productInvoices =
            await _unitOfWork.ProductInvoiceRepository.GetAsync(filter: pi => pi.ProductId == productId,
                includeProperties: "Invoice");
        if (productInvoices == null)
        {
            throw new Exception("No Invoice Found!");
        }

        return productInvoices.Select(pi => new InvoiceProductDto()
        {
            Id = pi.Invoice.Id,
            IssueDate = pi.Invoice.IssueDate,
            OwnerName = pi.Invoice.OwnerFirstName,
            OwnerFamilyName = pi.Invoice.OwnerLastName,
            IdentificationCode = pi.Invoice.IdentificationCode,
            IssuerName = pi.Invoice.IssuerName,
            PaymentDate = pi.Invoice.PaymentDate,
            PaymentStatus = pi.Invoice.PaymentStatus,
            TotalPrice = pi.Invoice.TotalPrice,
        });
    }
}