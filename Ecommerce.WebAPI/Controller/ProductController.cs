using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;


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
        var product = await _productService.GetProductByIdAsync(id);
        var product2 = await _productService.GetProductManufacturersAsync(id);
        var result = new
        {
            product.Id,
            product.Name,
            product.Inventory,
            product.Price,
            product.Status,
            product.DOP,
            product.DOE,
            Manufacturers = product2.ManufacturerProducts.Select(mp=> new
            {
                mp.Manufacturer.Id,
                mp.Manufacturer.Name
            })
        };
        if (product == null)
        {
            return NotFound($"The Product With Id {id} does not exist!");
        }

        return Ok(result);
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductAsync();
        return Ok(products);
    }

    [HttpGet("SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] String name)
    {
        var products = await _productService.GetAllProductsByNameAsync(name);
        return Ok(products);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddUpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
      var createdProduct =  await _productService.AddProductAsync(updateProductDto);
      return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct([FromQuery]Guid id, [FromBody] AddUpdateProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
        return Ok(updatedProduct);

    }
    

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
    {
        var deleted = await _productService.DeleteProductByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok($"The product with Id {id} successfully deleted");
    }
    
    
}