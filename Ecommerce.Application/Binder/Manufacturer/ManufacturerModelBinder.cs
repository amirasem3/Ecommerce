using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;


        AddUpdateManufacturerDto? addUpdateManufacturerDto;
        try
        {
            addUpdateManufacturerDto = JsonSerializer.Deserialize<AddUpdateManufacturerDto>(requestBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid JSON format.");
            return;
        }

        if (addUpdateManufacturerDto == null)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid invoice data.");
            return;
        }

        //Name
        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.Name))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Name is required.");
        }

        if (addUpdateManufacturerDto.Name.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Name cannot exceed 40 characters!");
        }

        if (!Regex.Match(addUpdateManufacturerDto.Name, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid characters in name!");
        }

        //Owner Name
        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.OwnerName))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Owner name is required.");
        }

        if (addUpdateManufacturerDto.OwnerName.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Owner name cannot exceed 40 characters!");
        }

        if (!Regex.Match(addUpdateManufacturerDto.OwnerName, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid characters in owner name!");
        }

        //Manufacturer Country

        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.ManufacturerCountry))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Manufacturer country is required.");
        }

        if (addUpdateManufacturerDto.ManufacturerCountry.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Manufacturer",
                "Manufacturer country cannot exceed 40 characters!");
        }

        if (!Regex.Match(addUpdateManufacturerDto.ManufacturerCountry, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase)
                .Success)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid characters in manufacturer name!");
        }

        //Email

        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.Email))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Email is required.");
        }

        if (addUpdateManufacturerDto.Email.Length > 20)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Email cannot exceed 20 characters!");
        }

        if (!addUpdateManufacturerDto.Email.Contains("@"))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Enter a valid email address!");
        }


        try
        {
            await _manufacturerService.GetManufacturerByEmailAsync(addUpdateManufacturerDto.Email);
            bindingContext.ModelState.AddModelError("Manufacturer", "Email is already exists!");
        }
        catch (Exception e)
        {
            if (e.Message != ManufacturerService.ManufacturerException)
            {
                bindingContext.ModelState.AddModelError("Manufacturer",
                    "An unexpected error occurred while checking the email uniqueness.");
            }
        }


        //Address

        if (addUpdateManufacturerDto.Address.Length > 100)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Address cannot exceed 40 characters!");
        }

        if (!Regex.Match(addUpdateManufacturerDto.Address, @"^[a-zA-Z0-9''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Invalid characters in address!");
        }

        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.Address))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Address is required.");
        }

        try
        {
            await _manufacturerService.GetManufacturerByAddressAsync(addUpdateManufacturerDto.Address);
            bindingContext.ModelState.AddModelError("Manufacturer", "Address is already exists.");
        }
        catch (Exception e)
        {
            if (e.Message!= ManufacturerService.ManufacturerException)
            {
                bindingContext.ModelState.AddModelError("Manufacturer","An unexpected error occurred while checking the address uniqueness.");
            }
        }
        

        //PhoneNumber


        try
        {
            var manufacturerExistPhoneNumber =
                await _manufacturerService.GetManufacturerByPhoneNumberAsync(addUpdateManufacturerDto.PhoneNumber);
            bindingContext.ModelState.AddModelError("Manufacturer", "Phone number is already exist.");
        }
        catch (Exception e)
        {
            if (e.Message!=ManufacturerService.ManufacturerException)
            {
                bindingContext.ModelState.AddModelError("Manufacturer", "An unexpected error occurred while checking the phone number uniqueness.");
            }
        }
        

        if (addUpdateManufacturerDto.PhoneNumber.Length > 20)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Phone number cannot exceed 40 characters!");
        }

        if (!Regex.Match(addUpdateManufacturerDto.PhoneNumber, @"^[0-9]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Manufacturer",
                "Non-numeric characters are not allowed in phone number");
        }

        if (string.IsNullOrWhiteSpace(addUpdateManufacturerDto.PhoneNumber))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Phone number is required.");
        }


        //Rate

        if (addUpdateManufacturerDto.Rate.GetType()!= typeof(int))
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Enter valid rate value!");
        }

        if (addUpdateManufacturerDto.Rate > 5)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Rate cannot be greater than 5!");
        }

        //Establish Date
        if (addUpdateManufacturerDto.EstablishDate == null)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Establish Date is required.");
        }

        if (addUpdateManufacturerDto.EstablishDate > DateTime.Now)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Enter valid establish date.");
        }

        //Status

        if (addUpdateManufacturerDto.Status == null)
        {
            bindingContext.ModelState.AddModelError("Manufacturer", "Status is required!");
        }

        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;

        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: addUpdateManufacturerDto);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(addUpdateManufacturerDto);
    }
}