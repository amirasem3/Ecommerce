using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace Ecommerce.Application.Services;

public class ProductService
{
    public const string ProductException = "Product Not Found!";
    private readonly UnitOfWork _unitOfWork;

    private readonly ILogger<ProductService> _logger;
    public ProductService(UnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a product by ID", args: [id]);
        var product = await _unitOfWork.productRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect Product ID", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails(_logger,"Product Found", args: [id], retrievedData: product);
        var resProd = new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"Target Product Found",args:[id],retrievedData:resProd);
        return resProd;
    }

    public async Task<ProductDto> AddProductAsync(AddUpdateProductDto newProductDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to add a new product", args: [newProductDto]);
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
        await _unitOfWork.productRepository.InsertAsync(product);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails(_logger,"Successful Product Insertion", args: [newProductDto], retrievedData: product);
        var resProd = new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"New Product added Successfully.",args:[newProductDto],retrievedData:resProd);
        return resProd;
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, AddUpdateProductDto updateProductDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to logger a product", args: [id, updateProductDto]);
        var product = await _unitOfWork.productRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this ID.", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Status = updateProductDto.Status;
        product.Inventory = updateProductDto.Inventory;
        product.Doe = updateProductDto.Doe;
        product.Dop = updateProductDto.Dop;

        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails(_logger,"Successful Update", args: [id, updateProductDto], retrievedData: product);
        var resProd = new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"Product Updated Successfully",args:[id,updateProductDto],retrievedData:resProd);
        return resProd;
    }

    public async Task<bool> DeleteProductByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to delete a product by ID.", args: [id]);
        var product = await _unitOfWork.productRepository.GetByIdAsync(id, "Manufacturers2");
        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this ID", args: [id], retrievedData: ProductException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        await _unitOfWork.productRepository.Delete(product);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Delete.", args: [id], retrievedData: product);
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to get all products");
        var products = await _unitOfWork.productRepository.GetAsync(includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Product table is empty.", retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails(_logger,"All Product Retrieved", retrievedData: products);

        var resProd =products.Select(product => new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"All Products retrieved successfully.",retrievedData:resProd);
        return resProd;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsByNameAsync(string name)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to search products by name", args: [name]);
        var products = await _unitOfWork.productRepository.GetAsync(filter: p => p.Name.Contains(name),
            includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this name.", args: [name],
                retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails(_logger,"All products with this name retrieved successfully.", args: [name],
            retrievedData: products);
        var resProd =products.Select(product => new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"Product's name search result",args:[name],retrievedData:resProd);
        return resProd;
    }

    public async Task<IEnumerable<ProductDto>> FilterProductByPriceAsync(decimal startPrice, decimal endPrice)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to filter products by the price", args: [startPrice, endPrice]);
        var products = await _unitOfWork.productRepository.GetAsync(
            filter: p => p.Price >= startPrice && p.Price <= endPrice, includeProperties: "Manufacturers2");
        if (products == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product in this price range.", args: [startPrice, endPrice],
                retrievedData: ProductException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ProductException);
        }

        LoggerHelper.LogWithDetails(_logger,"All product in the price range retrieved successfully.",
            args: [startPrice, endPrice], retrievedData: products);
        
        var resProd = products.Select(product => new ProductDto
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
        LoggerHelper.LogWithDetails(_logger,"Product's price filter results",args:[startPrice,endPrice],retrievedData:resProd);
        return resProd;
    }

    public async Task<IEnumerable<InvoiceProductDto>> GetInvoicesByProductId(Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get invoices with this product", args: [productId]);
        var productInvoices =
            await _unitOfWork.productInvoiceRepository.GetAsync(filter: pi => pi.ProductId == productId,
                includeProperties: "Invoice");
        if (productInvoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice that has this product.", args: [productId]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("No Invoice Found!");
        }

        LoggerHelper.LogWithDetails(_logger,"All invoices that have this product retrieved successfully.", args: [productId],
            retrievedData: productInvoices);
        
        var prodInvoiceRes =productInvoices.Select(pi => new InvoiceProductDto()
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
        LoggerHelper.LogWithDetails(_logger,"Invoice with this product.",args:[productId],retrievedData:prodInvoiceRes);
        return prodInvoiceRes;
    }
}