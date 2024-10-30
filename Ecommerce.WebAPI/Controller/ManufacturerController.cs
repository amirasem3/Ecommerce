﻿using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
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
    private readonly ManufacturerService _manufacturerService;

    public ManufacturerController(ManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    [HttpGet("GetManufacturerById/{id}")]
    [ActionName(nameof(GetManufacturerById))]
    public async Task<IActionResult> GetManufacturerById(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempts to get a manufacturer by ID", args: [id]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            LoggerHelper.LogWithDetails("Manufacturer DTO Result", args: [id], retrievedData: manufacturer);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Manufacturer ID", args: [id], retrievedData: e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllManufacturers")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        LoggerHelper.LogWithDetails("Attempts to get all manufacturers");

        try
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            LoggerHelper.LogWithDetails("All Manufacturers", retrievedData: manufacturers);
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Manufacturer table is empty", logLevel: LoggerHelper.LogLevel.Error,
                retrievedData: e.Message);
            return NotFound(e.Message);
        }
    }


    [HttpGet("GetManufacturerByAddress")]
    public async Task<IActionResult> SearchManufacturerAddresses([FromQuery] string address)
    {
        LoggerHelper.LogWithDetails("Attempts to get a manufacturer by address", args: [address]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByAddressAsync(address);
            LoggerHelper.LogWithDetails("Manufacturer DTO Result", args: [address], retrievedData: manufacturer);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("No Manufacturer with this address", args: [address], retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByEmail")]
    public async Task<IActionResult> SearchManufacturerEmails([FromQuery] string email)
    {
        LoggerHelper.LogWithDetails("Attempts to get a manufacturer by email", args: [email]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByEmailAsync(email);
            LoggerHelper.LogWithDetails("Manufacturer DTO Result", args: [email], retrievedData: manufacturer);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("No Manufacturer with this email", args: [email], retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByPhoneNumber")]
    public async Task<IActionResult> SearchManufacturerPhoneNumbers([FromQuery] string phoneNumber)
    {
        LoggerHelper.LogWithDetails("Attempts to get a manufacturer by phone number", args: [phoneNumber]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByPhoneNumberAsync(phoneNumber);
            LoggerHelper.LogWithDetails("Manufacturer DTO Result", args: [phoneNumber], retrievedData: manufacturer);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("No Manufacturer with this phone number", args: [phoneNumber],
                retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByOwner")]
    public async Task<IActionResult> SearchManufacturerOwners(string owner)
    {
        LoggerHelper.LogWithDetails("Attempts to get a manufacturer by owner name", args: [owner]);
        try
        {
            var manufacturers = await _manufacturerService.GetManufacturersByOwnerAsync(owner);
            LoggerHelper.LogWithDetails("Manufacturer DTO Result", args: [owner], retrievedData: manufacturers);
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("There is no manufacturer with this owner name", args: [owner],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddManufacturer")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddManufacturer([FromBody] AddUpdateManufacturerDto newManufacturer)
    {
        LoggerHelper.LogWithDetails("Attempt to add new manufacturer", args: [newManufacturer]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors", args: [newManufacturer],
                retrievedData: ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var manufacturer = await _manufacturerService.AddManufacturerAsync(newManufacturer);
            LoggerHelper.LogWithDetails("Manufacturer ADD DTO Result", args: [newManufacturer],
                retrievedData: manufacturer);
            return CreatedAtAction(nameof(GetManufacturerById), new { id = manufacturer.Id }, manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected errors", args: [newManufacturer],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("AssignManufacturerProduct")]
    public async Task<IActionResult> AssignManufacturerProduct([FromQuery] Guid manufacturerId,
        [FromQuery] Guid productId)
    {
        LoggerHelper.LogWithDetails("Attempt to add a product to a manufacturer", args: [productId, manufacturerId]);
        try
        {
            await _manufacturerService.AssignManufacturerProductsAsync(manufacturerId, productId);
            LoggerHelper.LogWithDetails("The product is successfully added to the manufacturers' products",
                args: [manufacturerId, productId]);
            return Ok("The product is successfully added to the manufacturers' products.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect manufacturer or product ID", args: [manufacturerId, productId],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPut("UpdateManufacturer/{id}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateManufacturer(Guid id,
        [FromBody] AddUpdateManufacturerDto updateManufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [id, updateManufacturerDto],
                retrievedData: ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedManufacturer = await _manufacturerService.UpdateManufacturerAsync(id, updateManufacturerDto);
            LoggerHelper.LogWithDetails("Update Result DTO", args: [id, updateManufacturerDto],
                retrievedData: updatedManufacturer);
            return Ok(updatedManufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect manufacturer ID", args: [id, updateManufacturerDto],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
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