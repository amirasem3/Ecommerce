using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace Ecommerce.Application.Services;

public class ManufacturerService
{
    private readonly UserService _userServices;
    private readonly UnitOfWork _unitOfWork;
    public const string ManufacturerException = "Manufacturer Not Found!";
    private readonly ILogger<ManufacturerService> _logger;


    public ManufacturerService(UserService userServices, UnitOfWork unitOfWork, ILogger<ManufacturerService>logger)
    {
        _userServices = userServices;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a manufacturer by ID", args: [id]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByIdAsync(id, "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this ID", args: [id],
                retrievedData: ManufacturerException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"Manufacturer Found", args: [id], retrievedData: manufacturer);
        var manRes = new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Email = manufacturer.Email,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EstablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            PhoneNumber = manufacturer.PhoneNumber,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        };
        LoggerHelper.LogWithDetails(_logger,"Target Manufacturer Found", args: [id], retrievedData: manRes);
        return manRes;
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync()
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to get all manufacturers");
        var manufacturers = await _unitOfWork.manufacturerRepository.GetAsync(includeProperties: "Products2");
        if (manufacturers == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Manufacturer table is empty.", retrievedData: ManufacturerException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"All manufacturers retrieved successfully", retrievedData: manufacturers);

        var manRes = manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Address = manufacturer.Address,
            EstablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        }).ToList();
        LoggerHelper.LogWithDetails(_logger,"All Manufacturers", retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByAddressAsync(string address)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a manufacturer by address", args: [address]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByUniquePropertyAsync(uniqueProperty: "Address",
            uniquePropertyValue: address, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this address", args: [address],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"Manufacturer Found", args: [address], retrievedData: manufacturer);
        var manRes = new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EstablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            Email = manufacturer.Email,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            PhoneNumber = manufacturer.PhoneNumber,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        };
        LoggerHelper.LogWithDetails(_logger,"Manufacturer's Address search result", args: [address], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByEmailAsync(string email)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a manufacturer by email", args: [email]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByUniquePropertyAsync(uniqueProperty: "Email",
            uniquePropertyValue: email, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this email", args: [email],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"Manufacturer Found", args: [email], retrievedData: manufacturer);
        var manRes =
            new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                OwnerName = manufacturer.OwnerName,
                Address = manufacturer.Address,
                EstablishDate = manufacturer.EstablishDate,
                Rate = manufacturer.Rate,
                Status = manufacturer.Status,
                ManufacturerCountry = manufacturer.ManufacturerCountry,
                Email = manufacturer.Email,
                PhoneNumber = manufacturer.PhoneNumber,
                Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
                {
                    Name = p.Name,
                    Doe = p.Doe,
                    Dop = p.Dop,
                    Inventory = p.Inventory,
                    Price = p.Price,
                    Status = p.Status
                }).ToList()
            };
        LoggerHelper.LogWithDetails(_logger,"Manufacturer's Email search result", args: [email], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a manufacturer by phone number", args: [phoneNumber]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByUniquePropertyAsync(
            uniqueProperty: "PhoneNumber", uniquePropertyValue: phoneNumber, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this phone number", args: [phoneNumber],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"Manufacturer Found", args: [phoneNumber], retrievedData: manufacturer);
        var manRes = new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            Email = manufacturer.Email,
            EstablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        };
        LoggerHelper.LogWithDetails(_logger,"Manufacturer's Phone Number search result", args: [phoneNumber],
            retrievedData: manRes);
        return manRes;
    }


    public async Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a manufacturer by owner name", args: [ownerName]);
        var manufacturers = await _unitOfWork.manufacturerRepository.GetAsync(
            filter: man => man.OwnerName.Contains(ownerName), includeProperties: "Products2");
        if (manufacturers == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this owner name", args: [ownerName],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails(_logger,"Manufacturers Found", args: [ownerName], retrievedData: manufacturers);
        var manRes = manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Address = manufacturer.Address,
            EstablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        }).ToList();
        LoggerHelper.LogWithDetails(_logger,"Manufacturer's Owner Name search result", args: [ownerName],
            retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto newManufacturerDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to add new manufacturer", args: [newManufacturerDto]);
        var manufacturer = new Manufacturer
        {
            Id = Guid.NewGuid(),
            Name = newManufacturerDto.Name,
            OwnerName = newManufacturerDto.OwnerName,
            ManufacturerCountry = newManufacturerDto.ManufacturerCountry,
            Email = newManufacturerDto.Email,
            Address = newManufacturerDto.Address,
            PhoneNumber = newManufacturerDto.PhoneNumber,
            Rate = newManufacturerDto.Rate,
            EstablishDate = newManufacturerDto.EstablishDate,
            Status = newManufacturerDto.Status,
            Products2 = []
        };
        await _unitOfWork.manufacturerRepository.InsertAsync(manufacturer);

        LoggerHelper.LogWithDetails(_logger,"Manufacturer Successful Insertion!", args: [newManufacturerDto],
            retrievedData: manufacturer);
        var manufacturerUser = new AddUpdateUserDto()
        {
            FirstName = manufacturer.Name,
            LastName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Username = manufacturer.Email,
            RoleName = "Manufacturer",
            PhoneNumber = manufacturer.PhoneNumber,
            Password = manufacturer.PhoneNumber
        };
        try
        {
            var manUser = await _userServices.AddUserAsync(manufacturerUser);
            LoggerHelper.LogWithDetails(_logger,"Manufacturer's user added successfully.", args: [manufacturerUser],
                retrievedData: manUser);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Create manufacturer user is Failed.", args: [manufacturerUser],
                retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(e.Message);
        }

        var manRes = new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = newManufacturerDto.Name,
            OwnerName = newManufacturerDto.OwnerName,
            ManufacturerCountry = newManufacturerDto.ManufacturerCountry,
            Email = newManufacturerDto.Email,
            Address = newManufacturerDto.Address,
            PhoneNumber = newManufacturerDto.PhoneNumber,
            Rate = newManufacturerDto.Rate,
            EstablishDate = newManufacturerDto.EstablishDate,
            Status = newManufacturerDto.Status,
            Products = []
        };
        LoggerHelper.LogWithDetails(_logger,"New Manufacturer", args: [newManufacturerDto], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,
        AddUpdateManufacturerDto updateManufacturerDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to update a manufacturer", args: [id, updateManufacturerDto]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByIdAsync(id, "Products2");

        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        manufacturer.Name = updateManufacturerDto.Name;
        manufacturer.OwnerName = updateManufacturerDto.OwnerName;
        manufacturer.Address = updateManufacturerDto.Address;
        manufacturer.EstablishDate = updateManufacturerDto.EstablishDate;
        manufacturer.Rate = updateManufacturerDto.Rate;
        manufacturer.Status = updateManufacturerDto.Status;
        manufacturer.PhoneNumber = updateManufacturerDto.PhoneNumber;
        manufacturer.Email = updateManufacturerDto.Email;
        manufacturer.ManufacturerCountry = updateManufacturerDto.ManufacturerCountry;
        // _unitOfWork.manufacturerRepository.Update(manufacturer);
        
        await _unitOfWork.SaveAsync();

        var manRes = new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EstablishDate = manufacturer.EstablishDate,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Email = manufacturer.Email,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            Products = manufacturer.Products2.Select(p => new ProductManufacturerDto
            {
                Name = p.Name,
                Doe = p.Doe,
                Dop = p.Dop,
                Inventory = p.Inventory,
                Price = p.Price,
                Status = p.Status
            }).ToList()
        };
        LoggerHelper.LogWithDetails(_logger,"Updated Manufacturer", args: [id, updateManufacturerDto], retrievedData: manRes);
        return manRes;
    }


    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to Delete a manufacturer by ID", args: [manufactureId]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByIdAsync(manufactureId, "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this ID", args: [manufactureId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        if (manufacturer.CheckProducts())
        {
            LoggerHelper.LogWithDetails(_logger,
                "There are products that relate to this manufacturer. " +
                "You cannot remove this manufacturer before all of its products",
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(
                "There are products that relate to this manufacturer. You cannot remove this manufacturer " +
                "before all of its products.");
        }

        // await _unitOfWork.manufacturerRepository.DeleteByIdAsync(manufactureId);
        await _unitOfWork.manufacturerRepository.Delete(manufacturer);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Delete", args: [manufactureId], retrievedData: manufacturer);
        return true;
    }

    public async Task AssignManufacturerProductsAsync(Guid manufacturerId, Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to add a product to a manufacturer", args: [manufacturerId, productId]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByIdAsync(manufacturerId, "Products2");
        var product = await _unitOfWork.productRepository.GetByIdAsync(productId);
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this ID", args: [manufacturerId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        bool productExists = manufacturer.Products2.Any(p => p.Id == productId);
        LoggerHelper.LogWithDetails(_logger,"Check existence of a product for a manufacturer",
            args: [manufacturer.Products2, productId], retrievedData: productExists);
        if (!productExists)
        {
            manufacturer.Products2.Add(product);
            // _unitOfWork.manufacturerRepository.Update(manufacturer);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails(_logger,"The product added to manufacture products successfully.",
                args: [product, manufacturer.Products2], retrievedData: manufacturer);
        }
        else
        {
            LoggerHelper.LogWithDetails(_logger,$"The product already exists in the {manufacturer.Name}'s products",
                args: [manufacturer],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception($"The product already exists in the {manufacturer.Name}'s products");
        }
    }

    public async Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to delete a product from a manufacturer's product",
            args: [manufacturerId, productId]);
        var manufacturer = await _unitOfWork.manufacturerRepository.GetByIdAsync(manufacturerId, "Products2");
        var product = await _unitOfWork.productRepository.GetByIdAsync(productId);
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this ID", args: [manufacturerId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        bool existProduct = manufacturer.Products2.Any(p => p.Id == productId);
        LoggerHelper.LogWithDetails(_logger,"Check existence of a product for a manufacturer",
            args: [manufacturer.Products2, productId], retrievedData: existProduct);
        if (existProduct)
        {
            manufacturer.Products2.Remove(product);
            // _unitOfWork.manufacturerRepository.Update(manufacturer);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails(_logger,"Successful Delete", args: [manufacturerId, productId]);
            return true;
        }

        LoggerHelper.LogWithDetails(_logger,"Unsuccessful Delete", args: [manufacturerId, productId]);
        throw new Exception("Unsuccessful Delete");
    }
}