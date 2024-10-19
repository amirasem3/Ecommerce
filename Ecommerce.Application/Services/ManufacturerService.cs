using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Services;

public class ManufacturerService : IManufacturerService
{

    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IManufacturerProductRepository _manufacturerProductRepository;
    private readonly IUserServices _userServices;
    private readonly IPasswordHasher<User> _passwordHasher;
    public ManufacturerService(IManufacturerRepository manufacturerRepository, IProductRepository productRepository,
        IManufacturerProductRepository manufacturerProductRepository, IUserServices userServices,
        IPasswordHasher<User> passwordHasher)
    {
        _manufacturerRepository = manufacturerRepository;
        _productRepository = productRepository;
        _manufacturerProductRepository = manufacturerProductRepository;
        _userServices = userServices;
        _passwordHasher = passwordHasher;
    }
     
    public async Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Email = manufacturer.Email,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<IEnumerable<ManufacturerDto>> SearchManufacturerByAddressAsync(string address)
    {
        var manufacturers = await _manufacturerRepository.SearchManufacturerByAddressAsync(address);
        
        return manufacturers.Select(manufacturer =>new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                OwnerName = manufacturer.OwnerName,
                Address = manufacturer.Address,
                EsatablishDate = manufacturer.EstablishDate,
                Rate = manufacturer.Rate,
                Status = manufacturer.Status,
                Email = manufacturer.Email,
                ManufacturerCountry = manufacturer.ManufacturerCountry,
                PhoneNumber = manufacturer.PhoneNumber,
                
            });
    }

    public async Task<IEnumerable<ManufacturerDto>> SearchManufacturerByEmailAsync(string email)
    {
        var manufacturers = await _manufacturerRepository.SearchManufacturerByEmailAsync(email);
        return manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Email = manufacturer.Email,
            PhoneNumber = manufacturer.PhoneNumber,
        });
    }

    public async Task<IEnumerable<ManufacturerDto>> SearchManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        var manufacturers = await _manufacturerRepository.SearchManufacturerByPhoneNumberAsync(phoneNumber);

        return manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            Email = manufacturer.Email,
            EsatablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
        });
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync()
    {
        var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();

        return manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
        });
    }

    public async Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName)
    {
        var manufacturers = await _manufacturerRepository.GetManufacturersByOwnerAsync(ownerName);

        return manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EstablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
            ManufacturerCountry = manufacturer.ManufacturerCountry
        });
    }

    public async Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto manufacturerDto)
    {
        var manufacturer = new Manufacturer
        {
            Id = Guid.NewGuid(),
            Name = manufacturerDto.Name,
            OwnerName = manufacturerDto.OwnerName,
            ManufacturerCountry = manufacturerDto.ManufacturerCountry,
            Email = manufacturerDto.Email,
            Address = manufacturerDto.Address,
            PhoneNumber = manufacturerDto.PhoneNumber,
            Rate = manufacturerDto.Rate,
            EstablishDate = manufacturerDto.EsatablishDate,
            Status = manufacturerDto.Status,
            Products = []
        };
        
        await _manufacturerRepository.AddManufacturerAsync(manufacturer);
        var manufacturerUser = new RegisterUserDto
        {
            FirstName = manufacturer.Name,
            LastName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Username = manufacturer.Email,
            RoleName = "Manufacturer",
            PhoneNumber = manufacturer.PhoneNumber,
            Password = manufacturer.PhoneNumber
        };
        await _userServices.AddUserAsync(manufacturerUser);
        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturerDto.Name,
            OwnerName = manufacturerDto.OwnerName,
            ManufacturerCountry = manufacturerDto.ManufacturerCountry,
            Email = manufacturerDto.Email,
            Address = manufacturerDto.Address,
            PhoneNumber = manufacturerDto.PhoneNumber,
            Rate = manufacturerDto.Rate,
            EsatablishDate = manufacturerDto.EsatablishDate,
            Status = manufacturerDto.Status,
            ManufacturerProducts = manufacturer.Products
        };
    }

    public async Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,AddUpdateManufacturerDto addUpdateManufacturerDto)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
       
            manufacturer.Name = addUpdateManufacturerDto.Name;
            manufacturer.OwnerName = addUpdateManufacturerDto.OwnerName;
            manufacturer.Address = addUpdateManufacturerDto.Address;
            manufacturer.EstablishDate = addUpdateManufacturerDto.EsatablishDate;
            manufacturer.Rate = addUpdateManufacturerDto.Rate;
            manufacturer.Status = addUpdateManufacturerDto.Status;
            manufacturer.PhoneNumber = addUpdateManufacturerDto.PhoneNumber;
            manufacturer.Email = addUpdateManufacturerDto.Email;
            manufacturer.ManufacturerCountry = addUpdateManufacturerDto.ManufacturerCountry;
            await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);
            return new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                OwnerName = manufacturer.OwnerName,
                Address = manufacturer.Address,
                EsatablishDate = manufacturer.EstablishDate,
                ManufacturerCountry = manufacturer.ManufacturerCountry,
                Email = manufacturer.Email,
                Rate = manufacturer.Rate,
                Status = manufacturer.Status,
                PhoneNumber = manufacturer.PhoneNumber,
                ManufacturerProducts = manufacturer.Products
            };
            

    }

   

    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufactureId);
        if (manufacturer != null)
        {
            await _manufacturerRepository.DeleteManufacturerAsync(manufactureId);
            return true;
        }

        return false;
    }

    public async Task<ManufacturerDto> GetManufactureProductAsync(Guid manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.GetManufactureProductAsync(manufacturerId);

        return new ManufacturerDto()
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Email = manufacturer.Email,
            Address = manufacturer.Address,
            PhoneNumber = manufacturer.PhoneNumber,
            EsatablishDate = manufacturer.EstablishDate,
            Status = manufacturer.Status,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Rate = manufacturer.Rate,
            ManufacturerProducts = manufacturer.Products
        };
    }
    
    public async Task AssignManufacturerProductsAsync(Guid manufacturerID, Guid productID)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerID);
        var product = await _productRepository.GetProductByIdAsync(productID);
        if (manufacturer == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (product == null)
        {
            throw new ArgumentException("Role not found.");
        }

        var exists = await _manufacturerProductRepository.ManufacturerHaveTheProductAsync(manufacturerID, productID);
        // Check if the user already has this role
        if (!exists)
        {
            var manufacturerProduct = new ManufacturerProduct
            {
                ManufacturerId = manufacturerID,
                ProductId = productID
            };

            await _manufacturerProductRepository.AddManufacturerProductAsync(manufacturerProduct);

        }
    }

    public async Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (manufacturer == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (product == null)
        {
            throw new ArgumentException("Role not found.");
        }

        return await _manufacturerProductRepository.DeleteManufacturerProductAsync(manufacturerId, productId);
       


    }
}