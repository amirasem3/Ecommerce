using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Application.Services;

public class ManufacturerService : IManufacturerService
{

    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IManufacturerProductRepository _manufacturerProductRepository;
    public ManufacturerService(IManufacturerRepository manufacturerRepository, IProductRepository productRepository, IManufacturerProductRepository manufacturerProductRepository)
    {
        _manufacturerRepository = manufacturerRepository;
        _productRepository = productRepository;
        _manufacturerProductRepository = manufacturerProductRepository;
    }
     
    public async Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<ManufacturerDto> SearchManufacturerByAddressAsync(string address)
    {
        var manufacturer = await _manufacturerRepository.SearchManufacturerByAddressAsync(address);

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<ManufacturerDto> SearchManufacturerByEmailAsync(string email)
    {
        var manufacturer = await _manufacturerRepository.SearchManufacturerByEmailAsync(email);
        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<ManufacturerDto> SearchManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        var manufacturer = await _manufacturerRepository.SearchManufacturerByPhoneNumberAsync(phoneNumber);

        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllManufacturersById()
    {
        var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();

        return manufacturers.Select(manufacturer => new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
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
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        });
    }

    public async Task<ManufacturerDto> AddManufacturerAsync(AddUpdateManufacturerDto addManufacturerDto)
    {
        var manufacturer = new Manufacturer
        {
            Id = Guid.NewGuid(),
            Name = addManufacturerDto.Name,
            OwnerName = addManufacturerDto.OwnerName,
            Address = addManufacturerDto.Address,
            Email = addManufacturerDto.Email,
            EsatablishDate = addManufacturerDto.EsatablishDate,
            ManufacturerCountry = addManufacturerDto.ManufacturerCountry,
            PhoneNumber = addManufacturerDto.PhoneNumber,
            Rate = addManufacturerDto.Rate,
            Status = addManufacturerDto.Status
        };
        await _manufacturerRepository.AddManufacturerAsync(manufacturer);
        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            OwnerName = manufacturer.OwnerName,
            Address = manufacturer.Address,
            EsatablishDate = manufacturer.EsatablishDate,
            Rate = manufacturer.Rate,
            Status = manufacturer.Status,
            PhoneNumber = manufacturer.PhoneNumber,
        };
    }

    public async Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,AddUpdateManufacturerDto updateManufacturerDto)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
       
            var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();
            manufacturer.Name = updateManufacturerDto.Name;
            manufacturer.OwnerName = updateManufacturerDto.OwnerName;
            manufacturer.Address = updateManufacturerDto.Address;
            manufacturer.EsatablishDate = updateManufacturerDto.EsatablishDate;
            manufacturer.Rate = updateManufacturerDto.Rate;
            manufacturer.Status = updateManufacturerDto.Status;
            manufacturer.PhoneNumber = updateManufacturerDto.PhoneNumber;
            await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);
            return new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                OwnerName = manufacturer.OwnerName,
                Address = manufacturer.Address,
                EsatablishDate = manufacturer.EsatablishDate,
                Rate = manufacturer.Rate,
                Status = manufacturer.Status,
                PhoneNumber = manufacturer.PhoneNumber,
            };
            

    }

    public async Task AssignProductToManufacturerAsync(Guid manufacturerID, Guid productID)
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

    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufactureId);
        if (manufacturer != null)
        {
            await _productRepository.DeleteProductByIdAsync(manufactureId);
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
            EsatablishDate = manufacturer.EsatablishDate,
            Status = manufacturer.Status,
            ManufacturerCountry = manufacturer.ManufacturerCountry,
            Rate = manufacturer.Rate,
        };
    }
}