using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services;

public class ManufacturerService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserService _userServices;
    public const string ManufacturerException = "Manufacturer Not Found!";

    public ManufacturerService(IManufacturerRepository manufacturerRepository, IProductRepository productRepository,
        UserService userServices)
    {
        _manufacturerRepository = manufacturerRepository;
        _productRepository = productRepository;
        _userServices = userServices;
    }

    public async Task<ManufacturerDto> GetManufacturerByIdAsync(Guid id)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        return new ManufacturerDto
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
    }

    public async Task<ManufacturerDto> GetManufacturerByAddressAsync(string address)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByAddressAsync(address);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        return new ManufacturerDto
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
        };
    }

    public async Task<ManufacturerDto> GetManufacturerByEmailAsync(string email)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByEmailAsync(email);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        return new ManufacturerDto
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
    }

    public async Task<ManufacturerDto> GetManufacturerByPhoneNumberAsync(string phoneNumber)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByPhoneNumberAsync(phoneNumber);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        return new ManufacturerDto
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
    }


    public async Task<IEnumerable<ManufacturerDto>> GetManufacturersByOwnerAsync(string ownerName)
    {
        var manufacturers = await _manufacturerRepository.GetManufacturersByOwnerAsync(ownerName);
        if (manufacturers == null)
        {
            throw new Exception(ManufacturerException);
        }

        return manufacturers.Select(manufacturer => new ManufacturerDto
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
            EstablishDate = manufacturerDto.EstablishDate,
            Status = manufacturerDto.Status,
            Products2 = []
        };

        await _manufacturerRepository.AddManufacturerAsync(manufacturer);
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
            await _userServices.AddUserAsync(manufacturerUser);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

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
            EstablishDate = manufacturerDto.EstablishDate,
            Status = manufacturerDto.Status,
            Products = []
        };
    }

    public async Task<ManufacturerDto> UpdateManufacturerAsync(Guid id,
        AddUpdateManufacturerDto addUpdateManufacturerDto)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        manufacturer.Name = addUpdateManufacturerDto.Name;
        manufacturer.OwnerName = addUpdateManufacturerDto.OwnerName;
        manufacturer.Address = addUpdateManufacturerDto.Address;
        manufacturer.EstablishDate = addUpdateManufacturerDto.EstablishDate;
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
    }


    public async Task<bool> DeleteManufacturerAsync(Guid manufactureId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufactureId);
        if (manufacturer == null)
        {
            throw new Exception(ManufacturerException);
        }

        if (manufacturer.Products2.Count != 0)
        {
            throw new Exception(
                "There are products that relate to this manufacturer. You cannot remove this manufacturer " +
                "before all of its products.");
        }

        await _manufacturerRepository.DeleteManufacturerAsync(manufactureId);
        return true;
    }

    public async Task AssignManufacturerProductsAsync(Guid manufacturerId, Guid productId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (manufacturer == null)
        {
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            throw new ArgumentException(ProductService.ProductException);
        }

        bool productExists = manufacturer.Products2.Any(p => p.Id == productId);
        if (!productExists)
        {
            await _manufacturerRepository.AddManufacturerProduct(manufacturer, product);
        }
        else
        {
            throw new Exception($"The product already exists in the {manufacturer.Name}'s products");
        }
    }

    public async Task<bool> DeleteManufacturerProductAsync(Guid manufacturerId, Guid productId)
    {
        var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (manufacturer == null)
        {
            throw new ArgumentException(ManufacturerException);
        }

        if (product == null)
        {
            throw new ArgumentException(ProductService.ProductException);
        }

        bool existProduct = manufacturer.Products2.Any(p => p.Id == productId);
        if (existProduct)
        {
            await _manufacturerRepository.DeleteManufacturerProductAsync(manufacturer, product);
            return true;
        }
        return false;
    }
}