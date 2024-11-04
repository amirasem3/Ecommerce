using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ProductService productService,ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect Product ID", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        LoggerHelper.LogWithDetails(_logger);
        try
        {
            var products = await _productService.GetAllProductAsync();
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", retrievedData: e, logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("FilterProductByPrice")]
    public async Task<IActionResult> FilterProductByPrice([FromQuery] decimal startPrice, [FromQuery] decimal endPrice)
    {
        LoggerHelper.LogWithDetails(_logger,args: [startPrice, endPrice]);
        try
        {
            var products = await _productService.FilterProductByPriceAsync(startPrice, endPrice);
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors.", args: [startPrice, endPrice], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        LoggerHelper.LogWithDetails(_logger,args: [name]);
        try
        {
            var products = await _productService.GetAllProductsByNameAsync(name);
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"No Product Found.", args: [name], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetProductInvoices/{productId}")]
    public async Task<IActionResult> GetInvoicesByProductId(Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,args: [productId]);
        try
        {
            var invoices = await _productService.GetInvoicesByProductId(productId);
            return Ok(invoices);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no Invoice with this product.", args: [productId], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddProduct([FromBody] AddUpdateProductDto newProductDto)
    {
        LoggerHelper.LogWithDetails(_logger,args: [newProductDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [newProductDto],
                retrievedData: ModelState["Product"]?.Errors.Select(e => e.ErrorMessage)!,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        var createdProduct = await _productService.AddProductAsync(newProductDto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("UpdateProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateProduct([FromQuery] Guid id, [FromBody] AddUpdateProductDto productDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to update a product", args: [id, productDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [id, productDto],
                retrievedData: ModelState["Product"]?.Errors.Select(e => e.ErrorMessage)!
                , logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            return Ok(updatedProduct);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect Product ID.", args: [id, productDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }


    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args:[id]);
        try
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok($"The product with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Incorrect Product ID.", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}