using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce.Application.Binder.Invoice;

public class InvoiceModelBinder : IModelBinder
{
    private readonly InvoiceServices _invoiceServices;

    public InvoiceModelBinder(InvoiceServices invoiceServices)
    {
        _invoiceServices = invoiceServices;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        bindingContext.HttpContext.Request.EnableBuffering();
        var requestBody = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        bindingContext.HttpContext.Request.Body.Position = 0;

    
        AddInvoiceDto? invoiceDto;
        try
        {
            invoiceDto = JsonSerializer.Deserialize<AddInvoiceDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            bindingContext.ModelState.AddModelError("Invoice", "Invalid JSON format.");
            return;
        }

        if (invoiceDto == null)
        {
            bindingContext.ModelState.AddModelError("Invoice", "Invalid invoice data.");
            return;
        }


        if (bindingContext.ModelMetadata.ModelType == typeof(AddInvoiceDto))
        {
            //Identification code
            if (string.IsNullOrWhiteSpace(invoiceDto.IdentificationCode))
            {
                bindingContext.ModelState.AddModelError("Invoice", "Identification code is required!");
                
            }

            if (invoiceDto.IdentificationCode!.Length > 40)
            {
                bindingContext.ModelState.AddModelError("identificationCode",
                    "Identification code cannot exceed 40 characters.");
                
            }

            if (!Regex.Match(invoiceDto.IdentificationCode, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
            {
                bindingContext.ModelState.AddModelError("Invoice",
                    "Invalid characters in identification code");
                
            }

            try
            {
                await _invoiceServices.GetInvoiceByIdentificationCodeAsync(invoiceDto.IdentificationCode);
                bindingContext.ModelState.AddModelError("Invoice",
                        "There is an invoice with same identification code!");
            }
            catch (Exception e)
            {
                if (e.Message!=InvoiceServices.InvoiceException)
                {
                    bindingContext.ModelState.AddModelError("Invoice", "An unexpected error occurred while checking the identification code uniqueness.");
                }
            }

            //Issuer Name value

            if (string.IsNullOrWhiteSpace(invoiceDto.IssuerName))
            {
                bindingContext.ModelState.AddModelError("Invoice", "Issuer name is required!");
                
            }

            if (invoiceDto.IssuerName.Length > 40)
            {
                bindingContext.ModelState.AddModelError("Invoice", "Issuer name cannot exceed 40 characters!");
                
            }

            if (!Regex.Match(invoiceDto.IssuerName, @"^[a-zA-Z'\s]+$", RegexOptions.IgnoreCase).Success)
            {
                bindingContext.ModelState.AddModelError("Invoice", "Invalid characters in issuer name.");
                
            }

            //Issuer Date
            
            if (invoiceDto.IssueDate > DateTime.Now)
            {
                bindingContext.ModelState.AddModelError("Invoice", "Enter valid issue date.");
                
            }
        }

        //Owner Name
        if (string.IsNullOrWhiteSpace(invoiceDto.OwnerName))
        {
            bindingContext.ModelState.AddModelError("Invoice", "Owner name is required!");
            
        }

        if (invoiceDto.OwnerName!.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Invoice", "Owner name cannot exceed 40 characters.");
            
        }

        if (!Regex.Match(invoiceDto.OwnerName, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Invoice", "Invalid characters in owner name!");
            
        }

        //Owner Family Name

        if (string.IsNullOrWhiteSpace(invoiceDto.OwnerFamilyName))
        {
            bindingContext.ModelState.AddModelError("Invoice", "Owner lastname is required!");
            
        }

        if (invoiceDto.OwnerFamilyName!.Length > 40)
        {
            bindingContext.ModelState.AddModelError("Invoice",
                "Owner family name cannot exceed 40 characters!");
            
        }

        if (!Regex.Match(invoiceDto.OwnerFamilyName, @"^[a-zA-Z'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("Invoice", "Invalid characters in owner family name.");
            
        }


        //total price 
        if (invoiceDto.TotalPrice.GetType() != typeof(decimal))
        {
            bindingContext.ModelState.AddModelError("Invoice", "Please enter valid value for total price.");
            
        }

        var objectValidator =
            (IObjectModelValidator)bindingContext.HttpContext.RequestServices.GetService(
                typeof(IObjectModelValidator))!;
        if (bindingContext.ModelMetadata.ModelType == typeof(UpdateInvoiceDto))
        {
            var updatedInvoice = new UpdateInvoiceDto()
            {
                OwnerName = invoiceDto.OwnerName,
                OwnerFamilyName = invoiceDto.OwnerFamilyName,
                TotalPrice = invoiceDto.TotalPrice
            };
            objectValidator!.Validate(
                actionContext: bindingContext.ActionContext,
                validationState: null,
                prefix: null!,
                model: updatedInvoice);
            bindingContext.Result = ModelBindingResult.Success(updatedInvoice);
            if (!bindingContext.ModelState.IsValid)
            {
                return;
            }

            return;
        }


        objectValidator!.Validate(
            actionContext: bindingContext.ActionContext,
            validationState: null,
            prefix: null!,
            model: invoiceDto);

        if (!bindingContext.ModelState.IsValid)
        {
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(invoiceDto);
    }
}