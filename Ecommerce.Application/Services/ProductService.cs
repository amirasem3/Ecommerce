using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Serilog;
using Serilog.Core;

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
        LoggerHelper.LogWithDetails("Attempt to get a product by ID", args: [id]);
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails("Incorrect Product ID", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails("Product Found", args: [id], retrievedData: product);
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

    public async Task<ProductDto> AddProductAsync(AddUpdateProductDto newProductDto)
    {
        LoggerHelper.LogWithDetails("Attempt to add a new product", args: [newProductDto]);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = newProductDto.Name,
            Price = newProductDto.Price,
            Inventory = newProductDto.Inventory,
            Dop = newProductDto.Dop,
            Doe = newProductDto.Doe,
            Status = newProductDto.Doe > newProductDto.Dop && newProductDto.Inventory > 0,
            Manufacturers2 = []
        };
        await _unitOfWork.ProductRepository.InsertAsync(product);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails("Successful Product Insertion", args: [newProductDto], retrievedData: product);
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
        LoggerHelper.LogWithDetails("Attempt to logger a product", args: [id, updateProductDto]);
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this ID.", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
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

        LoggerHelper.LogWithDetails("Successful Update", args: [id, updateProductDto], retrievedData: product);
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
        LoggerHelper.LogWithDetails("Attempts to delete a product by ID.", args: [id]);
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this ID", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        // await _productRepository.DeleteProductByIdAsync(id);
        await _unitOfWork.ProductRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Delete.",args:[id],retrievedData:product);
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
    {
        LoggerHelper.LogWithDetails("Attempts to get all products");
        var products = await _unitOfWork.ProductRepository.GetAsync(includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails("Product table is empty.", retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails("All Product Retrieved", retrievedData: products);

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
        LoggerHelper.LogWithDetails("Attempts to search products by name", args: [name]);
        var products = await _unitOfWork.ProductRepository.GetAsync(filter: p => p.Name.Contains(name),
            includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this name.", args: [name],
                retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails("All products with this name retrieved successfully.", args: [name],
            retrievedData: products);
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
        LoggerHelper.LogWithDetails("Attempt to filter products by the price", args: [startPrice, endPrice]);
        var products = await _unitOfWork.ProductRepository.GetAsync(
            filter: p => p.Price >= startPrice && p.Price <= endPrice, includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails("There is no product in this price range.", args: [startPrice, endPrice],
                retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails("All product in the price range retrieved successfully.",
            args: [startPrice, endPrice], retrievedData: products);

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
        LoggerHelper.LogWithDetails("Attempt to get invoices with this product", args: [productId]);
        var productInvoices =
            await _unitOfWork.ProductInvoiceRepository.GetAsync(filter: pi => pi.ProductId == productId,
                includeProperties: "Invoice");
        if (productInvoices == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice that has this product.", args: [productId]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("No Invoice Found!");
        }

        LoggerHelper.LogWithDetails("All invoices that have this product retrieved successfully.", args: [productId],
            retrievedData: productInvoices);
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