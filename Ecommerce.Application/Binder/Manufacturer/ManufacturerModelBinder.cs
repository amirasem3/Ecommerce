using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.Binder;

public class ManufacturerModelBinder : IModelBinder
{
    private readonly ManufacturerService _manufacturerService;

    public ManufacturerModelBinder(ManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var nameValue = bindingContext.ValueProvider.GetValue("name").FirstValue;
        var ownerNameValue = bindingContext.ValueProvider.GetValue("ownerName").FirstValue;
        var manufacturerCountryValue = bindingContext.ValueProvider.GetValue("manufacturerCountry").FirstValue;
        var emailValue = bindingContext.ValueProvider.GetValue("email").FirstValue;
        var addressValue = bindingContext.ValueProvider.GetValue("address").FirstValue;
        var phoneNumberValue = bindingContext.ValueProvider.GetValue("phoneNumber").FirstValue;
        var rateValue = bindingContext.ValueProvider.GetValue("rate").FirstValue;
        var establishDateValue = bindingContext.ValueProvider.GetValue("establishDate").FirstValue;
        var statusValue = bindingContext.ValueProvider.GetValue("status").FirstValue;

        //Name
        if (string.IsNullOrWhiteSpace(nameValue))
        {
            bindingContext.ModelState.AddModelError("name", "Name is required.");
            return;
        }

        if (nameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("name", "Name cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(nameValue, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("name", "Invalid characters in name!");
            return;
        }

        //Owner Name
        if (string.IsNullOrWhiteSpace(ownerNameValue))
        {
            bindingContext.ModelState.AddModelError("ownerName", "Owner name is required.");
            return;
        }
        if (ownerNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("ownerName", "Ownername cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(ownerNameValue, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("ownerName", "Invalid characters in owner name!");
            return;
        }
        
        //Manufacturer Country

        if (string.IsNullOrWhiteSpace(manufacturerCountryValue))
        {
            bindingContext.ModelState.AddModelError("manufacturerCountry", "Manufacturer country is required.");
            return;
        }
        if (manufacturerCountryValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("manufacturerCountry", "Manufacturer country cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(manufacturerCountryValue, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("manufacturerCountry", "Invalid characters in manufacturer name!");
            return;
        }
        
        //Email

        if (string.IsNullOrWhiteSpace(emailValue))
        {
            bindingContext.ModelState.AddModelError("email", "Email is required.");
            return;
        }
        if (emailValue.Length > 20)
        {
            bindingContext.ModelState.AddModelError("email", "Email cannot exceed 20 characters!");
            return;
        }

        if (!emailValue.Contains("@"))
        {
            bindingContext.ModelState.AddModelError("email", "Enter a valid email address!");
            return;
        }

        var manufacturerExistEmail = await _manufacturerService.SearchManufacturerByEmailAsync(emailValue);
        if (manufacturerExistEmail!=null)
        {
            bindingContext.ModelState.AddModelError("email", "Email is already exists!");
            return;
        }
        
        //Address
       
        if (addressValue.Length > 100)
        {
            bindingContext.ModelState.AddModelError("address", "Address cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(addressValue,  @"^[a-zA-Z0-9''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("address", "Invalid characters in address!");
            return;
        }

        if (string.IsNullOrWhiteSpace(addressValue))
        {
            bindingContext.ModelState.AddModelError("address", "Address is required.");
            return;
        }

        var manufacturerExistAddress = await _manufacturerService.SearchManufacturerByAddressAsync(addressValue);
        if (manufacturerExistAddress!=null)
        {
            bindingContext.ModelState.AddModelError("address", "Address is already exists." );
            return;
        }
        
        //PhoneNumber

        if (phoneNumberValue.Length > 20)
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "Phone number cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(phoneNumberValue,  @"^[0-9]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("phoneNumber", "Non-numeric characters are not allowed in phone number");
            return;
        }
        if (string.IsNullOrWhiteSpace(phoneNumberValue))
        {
            bindingContext.ModelState.AddModelError("phoneNumber","Phone number is required.");
            return;
        }
        
        
        //Rate

        if (!int.TryParse(rateValue, out int rate))
        {
            bindingContext.ModelState.AddModelError("rate", "Enter valid rate value!");
            return;
        }

        if (rate > 5)
        {
            bindingContext.ModelState.AddModelError("rate", "Rate cannot be greater than 5!");
            return;
        }

        //Establish Date
        if (string.IsNullOrWhiteSpace(establishDateValue))
        {
            bindingContext.ModelState.AddModelError("establishDate", "Establish Date is required.");
            return;
        }

        if (Convert.ToDateTime(establishDateValue) > DateTime.Now)
        {
            bindingContext.ModelState.AddModelError("establishDate", "Enter valid establish date.");
            return;
        }
        
        //Status

        if (string.IsNullOrWhiteSpace(statusValue))
        {
            bindingContext.ModelState.AddModelError("status", "Status is required!");
            return;
        }

        var manufacturer = new AddUpdateManufacturerDto()
        {
            Name = nameValue,
            Rate = rate,
            OwnerName = ownerNameValue,
            PhoneNumber = phoneNumberValue, EsatablishDate = Convert.ToDateTime(establishDateValue),
            Address = addressValue, ManufacturerCountry = manufacturerCountryValue, Status = Convert.ToBoolean(statusValue),
            Email = emailValue
        };
        
        bindingContext.Result = ModelBindingResult.Success(manufacturer);
    }
}