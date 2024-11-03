using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Serilog;
using Serilog.Core;

namespace Ecommerce.Application.Services;

public class ManufacturerService
{
    private readonly UserService _userServices;
    private readonly UnitOfWork _unitOfWork;
    public const string ManufacturerException = "Manufacturer Not Found!";


    public ManufacturerService(UserService userServices, UnitOfWork unitOfWork)
    {
        _userServices = userServices;
        _unitOfWork = unitOfWork;
    }

    public async Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a manufacturer by ID", args: [id]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id, "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this ID", args: [id],
                retrievedData: ManufacturerException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("Manufacturer Found", args: [id], retrievedData: manufacturer);
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
        LoggerHelper.LogWithDetails("Target Manufacturer Found", args: [id], retrievedData: manRes);
        return manRes;
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync()
    {
        LoggerHelper.LogWithDetails("Attempts to get all manufacturers");
        var manufacturers = await _unitOfWork.ManufacturerRepository.GetAsync(includeProperties: "Products2");
        if (manufacturers == null)
        {
            LoggerHelper.LogWithDetails("Manufacturer table is empty.", retrievedData: ManufacturerException,
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("All manufacturers retrieved successfully", retrievedData: manufacturers);

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
        LoggerHelper.LogWithDetails("All Manufacturers", retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByAddressAsync(string address)
    {
        LoggerHelper.LogWithDetails("Attempt to get a manufacturer by address", args: [address]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByUniquePropertyAsync(uniqueProperty: "Address",
            uniquePropertyValue: address, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this address", args: [address],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("Manufacturer Found", args: [address], retrievedData: manufacturer);
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
        LoggerHelper.LogWithDetails("Manufacturer's Address search result", args: [address], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByEmailAsync(string email)
    {
        LoggerHelper.LogWithDetails("Attempt to get a manufacturer by email", args: [email]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByUniquePropertyAsync(uniqueProperty: "Email",
            uniquePropertyValue: email, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this email", args: [email],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("Manufacturer Found", args: [email], retrievedData: manufacturer);
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
        LoggerHelper.LogWithDetails("Manufacturer's Email search result", args: [email], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> GetManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        LoggerHelper.LogWithDetails("Attempt to get a manufacturer by phone number", args: [phoneNumber]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByUniquePropertyAsync(
            uniqueProperty: "PhoneNumber", uniquePropertyValue: phoneNumber, includeProperties: "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this phone number", args: [phoneNumber],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("Manufacturer Found", args: [phoneNumber], retrievedData: manufacturer);
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
        LoggerHelper.LogWithDetails("Manufacturer's Phone Number search result", args: [phoneNumber],
            retrievedData: manRes);
        return manRes;
    }


    public async Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName)
    {
        LoggerHelper.LogWithDetails("Attempt to get a manufacturer by owner name", args: [ownerName]);
        var manufacturers = await _unitOfWork.ManufacturerRepository.GetAsync(
            filter: man => man.OwnerName.Contains(ownerName), includeProperties: "Products2");
        if (manufacturers == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this owner name", args: [ownerName],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        LoggerHelper.LogWithDetails("Manufacturers Found", args: [ownerName], retrievedData: manufacturers);
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
        LoggerHelper.LogWithDetails("Manufacturer's Owner Name search result", args: [ownerName],
            retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto newManufacturerDto)
    {
        LoggerHelper.LogWithDetails("Attempts to add new manufacturer", args: [newManufacturerDto]);
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
        await _unitOfWork.ManufacturerRepository.InsertAsync(manufacturer);
        await _unitOfWork.SaveAsync();

        LoggerHelper.LogWithDetails("Manufacturer Successful Insertion!", args: [newManufacturerDto],
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
            LoggerHelper.LogWithDetails("Manufacturer's user added successfully.", args: [manufacturerUser],
                retrievedData: manUser);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Create manufacturer user is Failed.", args: [manufacturerUser],
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
        LoggerHelper.LogWithDetails("New Manufacturer", args: [newManufacturerDto], retrievedData: manRes);
        return manRes;
    }

    public async Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,
        AddUpdateManufacturerDto updateManufacturerDto)
    {
        LoggerHelper.LogWithDetails("Attempt to update a manufacturer", args: [id, updateManufacturerDto]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id, "Products2");

        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this ID", args: [id],
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
        _unitOfWork.ManufacturerRepository.Update(manufacturer);
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
        LoggerHelper.LogWithDetails("Updated Manufacturer", args: [id, updateManufacturerDto], retrievedData: manRes);
        return manRes;
    }


    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        LoggerHelper.LogWithDetails("Attempt to Delete a manufacturer by ID", args: [manufactureId]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(manufactureId, "Products2");
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this ID", args: [manufactureId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(ManufacturerException);
        }

        if (manufacturer.Products2.Count != 0)
        {
            LoggerHelper.LogWithDetails(
                "There are products that relate to this manufacturer. " +
                "You cannot remove this manufacturer before all of its products",
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(
                "There are products that relate to this manufacturer. You cannot remove this manufacturer " +
                "before all of its products.");
        }

        await _unitOfWork.ManufacturerRepository.DeleteByIdAsync(manufactureId);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Delete", args: [manufactureId], retrievedData: manufacturer);
        return true;
    }

    public async Task AssignManufacturerProductsAsync(Guid manufacturerId, Guid productId)
    {
        LoggerHelper.LogWithDetails("Attempts to add a product to a manufacturer", args: [manufacturerId, productId]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(manufacturerId, "Products2");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this ID", args: [manufacturerId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        bool productExists = manufacturer.Products2.Any(p => p.Id == productId);
        LoggerHelper.LogWithDetails("Check existence of a product for a manufacturer",
            args: [manufacturer.Products2, productId], retrievedData: productExists);
        if (!productExists)
        {
            manufacturer.Products2.Add(product);
            _unitOfWork.ManufacturerRepository.Update(manufacturer);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails("The product added to manufacture products successfully.",
                args: [product, manufacturer.Products2], retrievedData: manufacturer);
        }
        else
        {
            LoggerHelper.LogWithDetails($"The product already exists in the {manufacturer.Name}'s products",
                args: [manufacturer],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception($"The product already exists in the {manufacturer.Name}'s products");
        }
    }

    public async Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId)
    {
        LoggerHelper.LogWithDetails("Attempt to delete a product from a manufacturer's product",
            args: [manufacturerId, productId]);
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(manufacturerId, "Products2");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        if (manufacturer == null)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this ID", args: [manufacturerId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        bool existProduct = manufacturer.Products2.Any(p => p.Id == productId);
        LoggerHelper.LogWithDetails("Check existence of a product for a manufacturer",
            args: [manufacturer.Products2, productId], retrievedData: existProduct);
        if (existProduct)
        {
            manufacturer.Products2.Remove(product);
            _unitOfWork.ManufacturerRepository.Update(manufacturer);
            await _unitOfWork.SaveAsync();
            LoggerHelper.LogWithDetails("Successful Delete", args: [manufacturerId, productId]);
            return true;
        }

        LoggerHelper.LogWithDetails("Unsuccessful Delete", args: [manufacturerId, productId]);
        throw new Exception("Unsuccessful Delete");
    }
}