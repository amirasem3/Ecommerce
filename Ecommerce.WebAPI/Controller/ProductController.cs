using Ecommerce.Application.DTOs;
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
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductAsync();
        return Ok(products);
    }

    [HttpGet("FilterProductByPrice")]
    public async Task<IActionResult> FilterProductByPrice([FromQuery] decimal startPrice, [FromQuery] decimal endPrice)
    {
        try
        {
            var products = await _productService.FilterProductByPriceAsync(startPrice, endPrice);
            return Ok(products);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        try
        {
            var products = await _productService.GetAllProductsByNameAsync(name);
            return Ok(products);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetProductInvoices/{productId}")]
    public async Task<IActionResult> GetInvoicesByProductId(Guid productId)
    {
        try
        {
            var invoices = await _productService.GetInvoicesByProductId(productId);
            return Ok(invoices);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddProduct([FromBody] AddUpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        var createdProduct = await _productService.AddProductAsync(updateProductDto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("UpdateProduct")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateProduct([FromQuery] Guid id, [FromBody] AddUpdateProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Product"]?.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            return Ok(updatedProduct);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
    {
        try
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok($"The product with Id {id} successfully deleted");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}