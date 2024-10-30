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

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a product by ID.", args: [id]);
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            LoggerHelper.LogWithDetails("Product DTO Result", args: [id], retrievedData: product);
            return Ok(product);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Product ID", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        LoggerHelper.LogWithDetails("Attempts to get all Products");

        try
        {
            var products = await _productService.GetAllProductAsync();
            LoggerHelper.LogWithDetails("All Products DTO", retrievedData: products);
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors", retrievedData: e, logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("FilterProductByPrice")]
    public async Task<IActionResult> FilterProductByPrice([FromQuery] decimal startPrice, [FromQuery] decimal endPrice)
    {
        LoggerHelper.LogWithDetails("Attempt to filter products by the price.", args: [startPrice, endPrice]);
        try
        {
            var products = await _productService.FilterProductByPriceAsync(startPrice, endPrice);
            LoggerHelper.LogWithDetails("All product in the price range found successfully.",
                args: [startPrice, endPrice], retrievedData: products);
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Unexpected Errors.", args: [startPrice, endPrice], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        LoggerHelper.LogWithDetails("Attempt to search products by the name.", args: [name]);
        try
        {
            var products = await _productService.GetAllProductsByNameAsync(name);
            LoggerHelper.LogWithDetails("Search DTO Result", args: [name], retrievedData: products);
            return Ok(products);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("No Product Found.", args: [name], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetProductInvoices/{productId}")]
    public async Task<IActionResult> GetInvoicesByProductId(Guid productId)
    {
        LoggerHelper.LogWithDetails("Attempts to get invoices with this product.", args: [productId]);
        try
        {
            var invoices = await _productService.GetInvoicesByProductId(productId);
            LoggerHelper.LogWithDetails("All Invoices DTO Result", args: [productId], retrievedData: invoices);
            return Ok(invoices);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("There is no Invoice with this product.", args: [productId], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddProduct([FromBody] AddUpdateProductDto newProductDto)
    {
        LoggerHelper.LogWithDetails("Attempt to add new product", args: [newProductDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [newProductDto],
                retrievedData: ModelState["Product"]?.Errors.Select(e => e.ErrorMessage)!,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        var createdProduct = await _productService.AddProductAsync(newProductDto);
        LoggerHelper.LogWithDetails("Product Add DTO Result", args: [newProductDto], retrievedData: createdProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("UpdateProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateProduct([FromQuery] Guid id, [FromBody] AddUpdateProductDto productDto)
    {
        LoggerHelper.LogWithDetails("Attempts to update a product", args: [id, productDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails("Binding Errors.", args: [id, productDto],
                retrievedData: ModelState["Product"]?.Errors.Select(e => e.ErrorMessage)!
                , logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            LoggerHelper.LogWithDetails("Product Update DTO Result", args: [id, productDto],
                retrievedData: updatedProduct);
            return Ok(updatedProduct);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Product ID.", args: [id, productDto], retrievedData: e,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }


    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to delete a product by ID",args:[id]);
        try
        {
            await _productService.DeleteProductByIdAsync(id);
            LoggerHelper.LogWithDetails($"The product with Id {id} successfully deleted",args:[id]);
            return Ok($"The product with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails("Incorrect Product ID.", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}