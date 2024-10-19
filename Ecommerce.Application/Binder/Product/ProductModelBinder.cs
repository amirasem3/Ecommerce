using System.Globalization;
using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        var nameValue = bindingContext.ValueProvider.GetValue("name").FirstValue;
        var priceValue = bindingContext.ValueProvider.GetValue("price").FirstValue;
        var inventoryValue = bindingContext.ValueProvider.GetValue("inventory").FirstValue;
        var dopValue = bindingContext.ValueProvider.GetValue("dop").FirstValue;
        var doeValue = bindingContext.ValueProvider.GetValue("doe").FirstValue;
        var statusValue = bindingContext.ValueProvider.GetValue("status").FirstValue;
        
        //Name
        if (string.IsNullOrWhiteSpace(nameValue))
        {
            bindingContext.ModelState.AddModelError("name", "Name is required.");
            return;
        }

        if (nameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("name", "Name cannot exceed 40 characters.");
            return;
        }

        if (!Regex.Match(nameValue, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("name", "Invalid characters in name!");
            return;
        }

        //Price
        if (string.IsNullOrWhiteSpace(priceValue))
        {
            bindingContext.ModelState.AddModelError("priceValue", "Price is required.");
            return;
        }
        
        if (!decimal.TryParse(priceValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
        {
            bindingContext.ModelState.AddModelError("price", "Invalid decimal value for Price.");
            return;
        }
        
        //Inventory
        if (string.IsNullOrWhiteSpace(inventoryValue))
        {
            bindingContext.ModelState.AddModelError("inventory", "Inventory is required.");
            return;
        }
        if (!decimal.TryParse(inventoryValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal inventory))
        {
            bindingContext.ModelState.AddModelError("price", "Invalid decimal value for Price.");
            return;
        }

        if (inventory <= 0)
        {
            bindingContext.ModelState.AddModelError("inventory", "Enter valid inventory!");
            return;
        }
        //DOP
        if (string.IsNullOrWhiteSpace(dopValue))
        {
            bindingContext.ModelState.AddModelError("dop", "Date of Production is required.");
            return;
        }

        //DOE
        if (string.IsNullOrWhiteSpace(doeValue))
        {
            bindingContext.ModelState.AddModelError("doe", "Data of Expiration is required.");
            return;
        }

        if (Convert.ToDateTime(doeValue) < Convert.ToDateTime(dopValue))
        {
            bindingContext.ModelState.AddModelError("status", "Invalid expiration date!");
            return;
        }
        
        
        //Status
        if (string.IsNullOrWhiteSpace(statusValue))
        {
            bindingContext.ModelState.AddModelError("status", "Status is required.");
            return;
        }
        if (!bool.TryParse(statusValue, out bool isAvailable))
        {
            bindingContext.ModelState.AddModelError("status", "Invalid boolean value for status.");
            return;
        }

        
        var addProduct = new AddUpdateProductDto()
        {
            Name = nameValue,
            Price = price,
            Doe = Convert.ToDateTime(doeValue),
            Dop = Convert.ToDateTime(dopValue),
            Inventory = inventory,
        };
        bindingContext.Result = ModelBindingResult.Success(addProduct);
    }
}