using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
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
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllManufacturers")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
        return Ok(manufacturers);
    }


    [HttpGet("GetManufacturerByAddress")]
    public async Task<IActionResult> SearchManufacturerAddresses([FromQuery] string address)
    {
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByAddressAsync(address);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByEmail")]
    public async Task<IActionResult> SearchManufacturerEmails([FromQuery] string email)
    {
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByEmailAsync(email);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByPhoneNumber")]
    public async Task<IActionResult> SearchManufacturerPhoneNumbers([FromQuery] string phoneNumber)
    {
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByPhoneNumberAsync(phoneNumber);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByOwner")]
    public async Task<IActionResult> SearchManufacturerOwners(string owner)
    {
        try
        {
            var manufacturers = await _manufacturerService.GetManufacturersByOwnerAsync(owner);
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddManufacturer")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddManufacturer([FromBody] AddUpdateManufacturerDto newManufacturer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var manufacturer = await _manufacturerService.AddManufacturerAsync(newManufacturer);
            return CreatedAtAction(nameof(GetManufacturerById), new { id = manufacturer.Id }, manufacturer);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("AssignManufacturerProduct")]
    public async Task<IActionResult> AssignManufacturerProduct([FromQuery] Guid manufacturerId,
        [FromQuery] Guid productId)
    {
        try
        {
            await _manufacturerService.AssignManufacturerProductsAsync(manufacturerId, productId);
            return Ok("The product is successfully added to the manufacturers' products.");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("UpdateManufacturer/{id}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateManufacturer(Guid id, [FromBody] AddUpdateManufacturerDto manufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedManufacturer = await _manufacturerService.UpdateManufacturerAsync(id, manufacturerDto);
            return Ok(updatedManufacturer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteManufacturer/{id}")]
    public async Task<IActionResult> DeleteManufacturer(Guid id)
    {
        try
        {
            await _manufacturerService.GetManufacturerByIdAsync(id);
            await _manufacturerService.DeleteManufacturerAsync(id);
            return Ok($"Manufacture with ID {id} has successfully deleted!");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteManufactureProduct")]
    public async Task<IActionResult> DeleteManufactureProducts([FromQuery] Guid manufacturerId,
        [FromQuery] Guid productId)
    {
        try
        {
            await _manufacturerService.DeleteManufacturerProductAsync(manufacturerId, productId);
            return Ok(
                $"The product with ID {productId} has successfully deleted from manufacturer with ID {manufacturerId}.");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}