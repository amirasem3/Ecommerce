using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;

[ApiController]
[Route("api/[controller]")]
public class ManufacturerController : ControllerBase
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    [HttpGet("GetManufacturerById/{id}")]
    [ActionName(nameof(GetManufacturerById))]
    public async Task<IActionResult> GetManufacturerById(Guid id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        var manufacturer2 = await _manufacturerService.GetManufactureProductAsync(id);
        var result = new
        {
            manufacturer.Id,
            manufacturer.Name,
            manufacturer.OwnerName,
            manufacturer.Email,
            manufacturer.PhoneNumber,
            manufacturer.Address,
            manufacturer.Rate,
            manufacturer.Status,
            manufacturer.ManufacturerCountry,
            Products  = manufacturer2.ManufacturerProducts.Select(mp => new
            {
                mp.Product.Id,
                mp.Product.Name
            })
        };
        if (manufacturer != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no Manufacturer with Id {id}.");
    }

    [HttpGet("GetAllManufacturers")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var manufacturers = await _manufacturerService.GetAllManufacturers();
        var result = new List<object>();
        foreach (var manufacturer in manufacturers)
        {
            var products = new List<object>();
            var man = await _manufacturerService.GetManufactureProductAsync(manufacturer.Id);
            foreach (var manProduct in man.ManufacturerProducts)
            {
                products.Add(new
                {
                    manProduct.Product.Id,
                    manProduct.Product.Name,
                });
            }
            
            result.Add(new
            {
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.OwnerName,
                manufacturer.Email,
                manufacturer.PhoneNumber,
                manufacturer.Address,
                manufacturer.EsatablishDate,
                manufacturer.Rate,
                manufacturer.Status,
                manufacturer.ManufacturerCountry,
                Products = products
            });
        }
        return Ok(result);
    }


    [HttpGet("GetManufacturerByAddress")]
    public async Task<IActionResult> SearchManufacturerAddresses([FromQuery] string address)
    {
        var manufacturers = await _manufacturerService.SearchManufacturerByAddressAsync(address);
        var result = new List<object>();
        foreach (var manufacturer in manufacturers)
        {
            var products = new List<object>();
            var man = await _manufacturerService.GetManufactureProductAsync(manufacturer.Id);
            foreach (var manProduct in man.ManufacturerProducts)
            {
                products.Add(new
                {
                    manProduct.Product.Id,
                    manProduct.Product.Name,
                });
            }
            
            result.Add(new
            {
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.OwnerName,
                manufacturer.Email,
                manufacturer.PhoneNumber,
                manufacturer.Address,
                manufacturer.Rate,
                manufacturer.Status,
                 manufacturer.ManufacturerCountry,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("GetManufacturerByEmail")]
    public async Task<IActionResult> SearchManufacturerEmails([FromQuery] string email)
    {
        var manufacturers = await _manufacturerService.SearchManufacturerByEmailAsync(email);

        var result = new List<object>();
        foreach (var manufacturer in manufacturers)
        {
            var products = new List<object>();
            var man = await _manufacturerService.GetManufactureProductAsync(manufacturer.Id);
            foreach (var manProduct in man.ManufacturerProducts)
            {
                products.Add(new
                {
                    manProduct.Product.Id,
                    manProduct.Product.Name,
                });
            }
            
            result.Add(new
            {
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.OwnerName,
                manufacturer.Email,
                manufacturer.PhoneNumber,
                manufacturer.Address,
                manufacturer.Rate,
                manufacturer.Status,
                manufacturer.ManufacturerCountry,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("GetManufacturerByPhoneNumber")]
    public async Task<IActionResult> SearchManufacturerPhoneNumbers([FromQuery] string phoneNumber)
    {
        var manufacturers = await _manufacturerService.SearchManufacturerByPhoneNumberAsync(phoneNumber);
        var result = new List<object>();
        foreach (var manufacturer in manufacturers)
        {
            var products = new List<object>();
            var man = await _manufacturerService.GetManufactureProductAsync(manufacturer.Id);
            foreach (var manProduct in man.ManufacturerProducts)
            {
                products.Add(new
                {
                    manProduct.Product.Id,
                    manProduct.Product.Name,
                });
            }
            
            result.Add(new
            {
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.OwnerName,
                manufacturer.Email,
                manufacturer.PhoneNumber,
                manufacturer.Address,
                manufacturer.Rate,
                manufacturer.Status,
                manufacturer.ManufacturerCountry,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("GetManufacturerByOwner")]
    public async Task<IActionResult> SearchManufacturerOwners(string owner)
    {
        var manufacturers = await _manufacturerService.GetManufacturersByOwnerAsync(owner);
        var result = new List<object>();
        foreach (var manufacturer in manufacturers)
        {
            var products = new List<object>();
            var man = await _manufacturerService.GetManufactureProductAsync(manufacturer.Id);
            foreach (var manProduct in man.ManufacturerProducts)
            {
                products.Add(new
                {
                    manProduct.Product.Id,
                    manProduct.Product.Name,
                });
            }
            
            result.Add(new
            {
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.OwnerName,
                manufacturer.Email,
                manufacturer.PhoneNumber,
                manufacturer.Address,
                manufacturer.Rate,
                manufacturer.Status,
                manufacturer.ManufacturerCountry,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpPost("AddManufacturer")]
    public async Task<IActionResult> AddManufacturer([FromBody] AddUpdateManufacturerDto newManufacturer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var manufacturer = await _manufacturerService.AddManufacturerAsync(newManufacturer);
        
        return CreatedAtAction(nameof(GetManufacturerById), new { id = manufacturer.Id }, manufacturer);
    }

    [HttpPost("AssignManufacturerProduct")]
    public async Task<IActionResult> AssignManufacturerProduct([FromQuery] Guid manufacturerId, [FromQuery] Guid productId)
    {
        await _manufacturerService.AssignManufacturerProductsAsync(manufacturerId, productId);
        return Ok(_manufacturerService.GetManufacturerByIdAsync(manufacturerId));
    }

    [HttpPut("UpdateManufacturer/{id}")]
    public async Task<IActionResult> UpdateManufacturer(Guid id, AddUpdateManufacturerDto manufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedManufacturer = await _manufacturerService.UpdateManufacturerAsync(id,manufacturerDto);
        return Ok(updatedManufacturer);
    }

    [HttpDelete("DeleteManufacturer/{id}")]
    public async Task<IActionResult> DeleteManufacturer(Guid id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
        var manufacturer2 = await _manufacturerService.GetManufactureProductAsync(id);
        
        if (manufacturer == null)
        {
            return NotFound($"There is no manufacturer with ID {id}");
        }

        if (manufacturer2.ManufacturerProducts.Count!=0)
        {
            return NotFound(
                "There are products that relate to this manufacturer. You cannot remove this manufacturer " +
                "before all of its products.");

        }

        await _manufacturerService.DeleteManufacturerAsync(id);
        return Ok($"Manufacture with ID {id} has successfully deleted!");
    }

    [HttpDelete("DeleteManufactureProduct")]
    public async Task<IActionResult> DeleteManufactureProducts([FromQuery] Guid manufacturerId, [FromQuery] Guid productId)
    {
        var result = await _manufacturerService.DeleteManufacturerProductAsync(manufacturerId, productId);

        if (result)
        {
            return Ok($"The product with ID {productId} has successfully deleted from manufacturer with ID {manufacturerId}.");
        }

        return NotFound("Invalid Manifacturer Id or productId");
    }
}