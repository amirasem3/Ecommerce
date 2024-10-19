using System.Text.RegularExpressions;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

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
        var ownerNameValue = bindingContext.ValueProvider.GetValue("ownerName").FirstValue;
        var ownerFamilyNameValue = bindingContext.ValueProvider.GetValue("ownerFamilyName").FirstValue;
        var identificationCodeValue = bindingContext.ValueProvider.GetValue("identificationCode").FirstValue;
        var issuerNameValue = bindingContext.ValueProvider.GetValue("issuerName").FirstValue;
        var issueDateValue = bindingContext.ValueProvider.GetValue("issueDate").FirstValue;
        var totalPriceValue = bindingContext.ValueProvider.GetValue("totalPrice").FirstValue;


        if (bindingContext.ModelMetadata.ModelType == typeof(AddInvoiceDto))
        {
             //Identification code
        if (string.IsNullOrWhiteSpace(identificationCodeValue))
        {
            bindingContext.ModelState.AddModelError("identificationCode", "Identification code is required!");
            return;
        }

        if (identificationCodeValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("identificationCode", "Identification code cannot exceed 40 characters.");
            return;
        }

        if (!Regex.Match(identificationCodeValue, @"^[a-zA-Z0-9''-'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("identificationCode", "Invalid characters in identification code");
            return;
        }

        var invoice = await _invoiceServices.GetInvoiceByIdentificationCodeAsync(identificationCodeValue);
        if (invoice!=null)
        {
            bindingContext.ModelState.AddModelError("identificationCode", "There is an invoice with same identification code!");
            return;
        }
        
        //Issuer Name value

        if (string.IsNullOrWhiteSpace(issuerNameValue))
        {
            bindingContext.ModelState.AddModelError("issuerName", "Issuer name is required!");
            return;
        }

        if (issuerNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("issuerName", "Issuer name cannot exceed 40 characters!" );
            return;
        }

        if (!Regex.Match(issuerNameValue, @"^[a-zA-Z0-9'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("issuerName", "Invalid characters in issuer name.");
            return;
        }
        
        //Issuer Date

        if (string.IsNullOrWhiteSpace(issueDateValue))
        {
            bindingContext.ModelState.AddModelError("issueDate", "Issue date is required!");
            return;
        }
        if (Convert.ToDateTime(issueDateValue) > DateTime.Now)
        {
            bindingContext.ModelState.AddModelError("issueDate", "Enter valid issue date.");
            return;
        }
        }
        
        //Owner Name
        if (string.IsNullOrWhiteSpace(ownerNameValue))
        {
            bindingContext.ModelState.AddModelError("ownerName", "Owner name is required!");
            return;
        }

        if (ownerNameValue.Length> 40)
        {
            bindingContext.ModelState.AddModelError("ownerName", "Owner name cannot exceed 40 characters.");
            return;
        }

        if (!Regex.Match(ownerNameValue, @"^[a-zA-Z''\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("ownerName", "Invalid characters in owner name!");
            return;
        }
        
        //Owner Family Name

        if (string.IsNullOrWhiteSpace(ownerFamilyNameValue))
        {
            bindingContext.ModelState.AddModelError("ownerFamilyName", "Owner lastname is required!");
            return;
        }

        if (ownerFamilyNameValue.Length > 40)
        {
            bindingContext.ModelState.AddModelError("ownerFamilyName", "Owner family name cannot exceed 40 characters!");
            return;
        }

        if (!Regex.Match(ownerFamilyNameValue, @"^[a-zA-Z'\s]+$", RegexOptions.IgnoreCase).Success)
        {
            bindingContext.ModelState.AddModelError("ownerFamilyName", "Invalid characters in owner family name.");
            return;
        }
        
       
        
        //total price 
        if (!decimal.TryParse(totalPriceValue, out decimal totalPrice))
        {
            bindingContext.ModelState.AddModelError("totalPrice", "Please enter valid value for total price.");
            return;
        }

        if (bindingContext.ModelMetadata.ModelType == typeof(UpdateInvoiceDto))
        {
            var updatedInvoice = new UpdateInvoiceDto()
            {
                OwnerName = ownerNameValue,
                OwnerFamilyName = ownerNameValue,
                TotalPrice = totalPrice
            };
            bindingContext.Result = ModelBindingResult.Success(updatedInvoice);
            return;
        }

        var newInvoice = new AddInvoiceDto()
        {
            OwnerName = ownerNameValue,
            OwnerFamilyName = ownerFamilyNameValue,
            IssuerName = issuerNameValue,
            IssueDate = Convert.ToDateTime(issueDateValue),
            IdentificationCode = identificationCodeValue,
            TotalPrice = totalPrice
        };
        bindingContext.Result = ModelBindingResult.Success(newInvoice);
    }
}