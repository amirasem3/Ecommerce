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
    private readonly ILogger<ManufacturerController> _logger;

    public ManufacturerController(ManufacturerService manufacturerService, ILogger<ManufacturerController> logger)
    {
        _manufacturerService = manufacturerService;
        _logger = logger;
    }

    [HttpGet("GetManufacturerById/{id}")]
    [ActionName(nameof(GetManufacturerById))]
    public async Task<IActionResult> GetManufacturerById(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect Manufacturer ID", args: [id], retrievedData: e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllManufacturers")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        LoggerHelper.LogWithDetails(_logger);

        try
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Manufacturer table is empty", logLevel: LoggerHelper.LogLevel.Error,
                retrievedData: e.Message);
            return NotFound(e.Message);
        }
    }


    [HttpGet("GetManufacturerByAddress")]
    public async Task<IActionResult> SearchManufacturerAddresses([FromQuery] string address)
    {
        LoggerHelper.LogWithDetails(_logger,args: [address]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByAddressAsync(address);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"No Manufacturer with this address", args: [address], retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByEmail")]
    public async Task<IActionResult> SearchManufacturerEmails([FromQuery] string email)
    {
        LoggerHelper.LogWithDetails(_logger,args: [email]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByEmailAsync(email);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"No Manufacturer with this email", args: [email], retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByPhoneNumber")]
    public async Task<IActionResult> SearchManufacturerPhoneNumbers([FromQuery] string phoneNumber)
    {
        LoggerHelper.LogWithDetails(_logger,args: [phoneNumber]);
        try
        {
            var manufacturer = await _manufacturerService.GetManufacturerByPhoneNumberAsync(phoneNumber);
            return Ok(manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"No Manufacturer with this phone number", args: [phoneNumber],
                retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetManufacturerByOwner")]
    public async Task<IActionResult> SearchManufacturerOwners(string owner)
    {
        LoggerHelper.LogWithDetails(_logger,args: [owner]);
        try
        {
            var manufacturers = await _manufacturerService.GetManufacturersByOwnerAsync(owner);
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no manufacturer with this owner name", args: [owner],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddManufacturer")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddManufacturer([FromBody] AddUpdateManufacturerDto newManufacturer)
    {
        LoggerHelper.LogWithDetails(_logger,args: [newManufacturer]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors", args: [newManufacturer],
                retrievedData: ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var manufacturer = await _manufacturerService.AddManufacturerAsync(newManufacturer);
            return CreatedAtAction(nameof(GetManufacturerById), new { id = manufacturer.Id }, manufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected errors", args: [newManufacturer],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("AssignManufacturerProduct")]
    public async Task<IActionResult> AssignManufacturerProduct([FromQuery] Guid manufacturerId,
        [FromQuery] Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,args: [productId, manufacturerId]);
        try
        {
            await _manufacturerService.AssignManufacturerProductsAsync(manufacturerId, productId);
            return Ok("The product is successfully added to the manufacturers' products.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect manufacturer or product ID", args: [manufacturerId, productId],
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
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [id, updateManufacturerDto],
                retrievedData: ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage),
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Manufacturer"]!.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedManufacturer = await _manufacturerService.UpdateManufacturerAsync(id, updateManufacturerDto);
            return Ok(updatedManufacturer);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect manufacturer ID", args: [id, updateManufacturerDto],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteManufacturer/{id}")]
    public async Task<IActionResult> DeleteManufacturer(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            await _manufacturerService.DeleteManufacturerAsync(id);
            return Ok($"Manufacture with ID {id} has successfully deleted!");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteManufactureProduct")]
    public async Task<IActionResult> DeleteManufactureProducts([FromQuery] Guid manufacturerId,
        [FromQuery] Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,args: [manufacturerId, productId]);
        try
        {
            await _manufacturerService.DeleteManufacturerProductAsync(manufacturerId, productId);
            return Ok(
                $"The product with ID {productId} has successfully deleted from manufacturer with ID {manufacturerId}.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [manufacturerId, productId], retrievedData: e.Message
                , logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}