using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce.Application.Binder.Product;

public class ProductModelBinder : IModelBinder
{

    private readonly ProductService _productService;

    public ProductModelBinder(ProductService productService)
    {
        _productService = productService;
    }
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;

    
        AddUpdateProductDto? addUpdateProductDto;
        try
        {
            addUpdateProductDto = JsonSerializer.Deserialize<AddUpdateProductDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid JSON format.");
            return;
        }

        if (addUpdateProductDto == null)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid invoice data.");
            return;
        }
        //Name
        if (string.IsNullOrWhiteSpace(addUpdateProductDto.Name))
        {
            bindingContext.ModelState.AddModelError("Product", "Name is required.");
          
        }

        if (addUpdateProductDto.Name.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Product", "Name cannot exceed 40 characters.");
           
        }

        if (!Regex.Match(addUpdateProductDto.Name, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Product", "Invalid characters in name!");
           
        }

        //Price
        if (addUpdateProductDto.Price == null)
        {
            bindingContext.ModelState.AddModelError("Product", "Price is required.");
        }
        
        if (addUpdateProductDto.Price.GetType() != typeof(decimal))
        {
            bindingContext.ModelState.AddModelError("Product", "Invalid decimal value for Price.");
            
        }
        
        //Inventory
        if (addUpdateProductDto.Inventory == null)
        {
            bindingContext.ModelState.AddModelError("Product", "Inventory is required.");
        }
        if (addUpdateProductDto.Inventory.GetType() != typeof(decimal))
        {
            bindingContext.ModelState.AddModelError("Product", "Invalid decimal value for Price.");
        }

        if (addUpdateProductDto.Inventory <= 0)
        {
            bindingContext.ModelState.AddModelError("Product", "Enter valid inventory!");
        }
        //DOP
        if (addUpdateProductDto.Dop == null)
        {
            bindingContext.ModelState.AddModelError("Product", "Date of Production is required.");
        }

        //DOE
        if (addUpdateProductDto.Doe == null)
        {
            bindingContext.ModelState.AddModelError("Product", "Data of Expiration is required.");
        }

        if (addUpdateProductDto.Doe < addUpdateProductDto.Dop)
        {
            bindingContext.ModelState.AddModelError("Product", "Invalid expiration date!");
        }
        
        
        //Status
        if (addUpdateProductDto.Status.GetType() != typeof(bool))
        {
            bindingContext.ModelState.AddModelError("Product", "Status is required.");
        }
        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;
        
        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: addUpdateProductDto);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(addUpdateProductDto);
    }
}