using Ecommerce.Application.Binder.Product;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IInvoiceServices _invoiceServices;

    public ProductController(IProductService productService, IInvoiceServices invoiceServices)
    {
        _productService = productService;
        _invoiceServices = invoiceServices;
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
            Manufacturers = product2.ManufacturerProducts.Select(mp => new
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
        var result = new List<object>();
        foreach (var product in products)
        {
            var manufacturers = new List<object>();
            var prod = await _productService.GetProductManufacturersAsync(product.Id);
            foreach (var manProduct in prod.ManufacturerProducts)
            {
                manufacturers.Add(new
                {
                    manProduct.Manufacturer.Id,
                    manProduct.Manufacturer.Name
                });
            }

            result.Add(new
            {
                product.Id,
                product.Name,
                product.Inventory,
                product.Price,
                product.Status,
                product.DOP,
                product.DOE,
                Manufacturers = manufacturers
            });
        }

        return Ok(result);
    }

    [HttpGet("FilterProductByPrice")]
    public async Task<IActionResult> FilterProductByPrice([FromQuery] decimal startPrice, [FromQuery] decimal endPrice)
    {
        var products = await _productService.FilterProductByPriceAsync(startPrice, endPrice);
        var result = new List<object>();
        foreach (var product in products)
        {
            var manufacturers = new List<object>();
            var prod = await _productService.GetProductManufacturersAsync(product.Id);
            foreach (var manProduct in prod.ManufacturerProducts)
            {
                manufacturers.Add(new
                {
                    manProduct.Manufacturer.Id,
                    manProduct.Manufacturer.Name
                });
            }
            
            result.Add(new
            {
                product.Id,
                product.Name,
                product.Inventory,
                product.Price,
                product.Status,
                product.DOP,
                product.DOE,
                Manufacturers = manufacturers
            });
        }
        return Ok(result);
    }

[HttpGet("SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] String name)
    {
        var products = await _productService.GetAllProductsByNameAsync(name);
        var result = new List<object>();
        foreach (var product in products)
        {
            var manufacturers = new List<object>();
            var prod = await _productService.GetProductManufacturersAsync(product.Id);
            foreach (var manProduct in prod.ManufacturerProducts)
            {
                manufacturers.Add(new
                {
                    manProduct.Manufacturer.Id,
                    manProduct.Manufacturer.Name
                });
            }
            
            result.Add(new
            {
                product.Id,
                product.Name,
                product.Inventory,
                product.Price,
                product.Status,
                product.DOP,
                product.DOE,
                Manufacturers = manufacturers
            });
        }
        return Ok(result);
    }

    [HttpGet("GetProductInvoices/{productId}")]
    public async Task<IActionResult> GetProductInvoices(Guid productId)
    {
        var productInvoices = await _productService.GetProductInvoicesAsync(productId);
        var result = new List<object>();
        foreach (var pi in productInvoices.ProductInvoices)
        {
            var invoices = new List<object>();
            var prod = await _productService.GetProductInvoicesAsync(pi.ProductId);
            var invoice = await _invoiceServices.GetInvoiceByIdAsync(pi.InvoiceId);
            result.Add(invoice);
        }
        return Ok(result);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([ModelBinder(typeof(ProductModelBinder))] AddUpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { Errors = errors });
        }
        
      var createdProduct =  await _productService.AddProductAsync(updateProductDto);
      return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct([FromQuery]Guid id, [ModelBinder(typeof(ProductModelBinder))] AddUpdateProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { Errors = errors });
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