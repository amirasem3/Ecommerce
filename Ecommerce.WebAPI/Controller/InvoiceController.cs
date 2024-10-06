using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSolution.Controller;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceServices _invoiceServices;

    public InvoiceController(IInvoiceServices invoiceServices)
    {
        _invoiceServices = invoiceServices;
    }

    [HttpGet("GetInvoiceById/{id}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        var invoice = await _invoiceServices.GetInvoiceByIdAsync(id);
        var invoiceProduct = await _invoiceServices.GetInvoiceProductAsync(id);
        var result = new
        {
            invoice.Id,
            invoice.OwnerName,
            invoice.IdentificationCode,
            invoice.OwnerFamilyName,
            invoice.IssuerName,
            invoice.IssueDate,
            payment_date = invoice.PaymentDate!=null ? invoice.PaymentDate.ToString() : "Not Payed",
            payment_status = invoice.PaymentStatus.ToString("G"),
            invoice.TotalPrice,
            Products = invoiceProduct.ProductInvoices.Select(pi => new
            {
                pi.ProductId,
                pi.Product.Name,
                pi.Product.Price,
                pi.Count
            })
        };
        if (invoice != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no invoice with Id {id}");
    }

    [HttpGet("GetInvoiceByIDNumber")]
    public async Task<IActionResult> GetInvoiceByIdNumber([FromQuery] string idNumber)
    {
        var invoice = await _invoiceServices.GetInvoiceByIdentificationCodeAsync(idNumber);
        var invoiceProduct = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
        var result = new
        {
            invoice.Id,
            invoice.OwnerName,
            invoice.IdentificationCode,
            invoice.OwnerFamilyName,
            invoice.IssuerName,
            invoice.IssueDate,
            invoice.PaymentDate,
            payment_status = invoice.PaymentStatus.ToString("G"),
            invoice.TotalPrice,
            Products = invoiceProduct.ProductInvoices.Select(pi => new
            {
                pi.ProductId,
                pi.Product.Name,
                pi.Product.Price,
                pi.Count
            })
        };
        if (invoice != null)
        {
            return Ok(result);
        }

        return NotFound($"There is no invoice with Id {invoice.Id}");
    }

    [HttpGet("SearchInvoiceByOwnerName/{ownerName}")]
    public async Task<IActionResult> SearchInvoicesByOwnerName(string ownerName)
    {
        var Invoices = await _invoiceServices.GetInvoicesByOwnerNameAsync(ownerName);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }

            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }

        return Ok(result);

    }

    [HttpGet("SearchInvoiceByOwnerLastName/{lastname}")]
    public async Task<IActionResult> SearchInvoicesByOwnerLastname(string lastname)
    {
        var Invoices = await _invoiceServices.GetInvoicesByOwnerFamilyNameAsync(lastname);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("SearchInvoicesByIssuerName/{issuerName}")]
    public async Task<IActionResult> SearchInvoicesByIssuerName(string issuerName)
    {
        var Invoices = await _invoiceServices.GetInvoicesByIssuerNameAsync(issuerName);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("PaymentStatusFilter/{paymentStatus}")]
    public async Task<IActionResult> PaymentStatusFiler(string paymentStatus)
    {
        var Invoices = await _invoiceServices.GetInvoicesByPaymentStatusAsync(paymentStatus);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("IssueDateFilter/{issueDate}")]
    public async Task<IActionResult> IssueDateFilter(DateTime issueDate)
    {
        var Invoices = await _invoiceServices.GetInvoiceByIssueDateAsync(issueDate);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }

    [HttpGet("PaymentDataFilter/{paymentDate}")]
    public async Task<IActionResult> PaymentDateFilter(DateTime paymentDate)
    {
        var Invoices = await _invoiceServices.GetInvoicesByPaymentDateAsync(paymentDate);
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }

[HttpGet("GetAllInvoices")]
    public async Task<IActionResult> GetAllInvoices()
    {
        var Invoices = await _invoiceServices.GetAllInvoicesAsync();
        var result = new List<object>();
        foreach (var invoice in Invoices)
        {
            var products = new List<object>();
            var inv = await _invoiceServices.GetInvoiceProductAsync(invoice.Id);
            foreach (var invProducts in inv.ProductInvoices)
            {
                products.Add(new
                {
                    invProducts.Product.Id,
                    invProducts.Product.Name,
                    invProducts.Product.Price,
                    invProducts.Count
                });
            }
            
            result.Add(new
            {
                invoice.Id,
                invoice.OwnerName,
                invoice.IdentificationCode,
                invoice.OwnerFamilyName,
                invoice.IssuerName,
                invoice.IssueDate,
                invoice.PaymentDate,
                payment_status = invoice.PaymentStatus.ToString("G"),
                invoice.TotalPrice,
                Products = products
            });
        }
        return Ok(result);
    }
    [HttpPost("IssueNewInvoice")]
    public async Task<IActionResult> IssueNewInvoice([FromBody] AddInvoiceDto newInvoice)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var invoice = await _invoiceServices.AddInvoiceAsync(newInvoice);
        return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
    }

    [HttpPost("AddInvoiceProducts")]
    public async Task<IActionResult> AddProductToInvoice([FromQuery] Guid invoiceId, [FromQuery] Guid productId, [FromQuery] int count)
    {
        await _invoiceServices.AssignInvoiceProductAsync(invoiceId, productId, count);
        return Ok(_invoiceServices.GetInvoiceByIdAsync(invoiceId));
    }

    [HttpPut("UpdateInvoice/{id}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] UpdateInvoiceDto updateInvoiceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedInvoice = await _invoiceServices.UpdateInvoiceAsync(id, updateInvoiceDto);
        return Ok(updatedInvoice);
    }

    [HttpPut("Pay")]
    public async Task<IActionResult> PayInvoice([FromQuery] Guid invoiceId, [FromQuery] decimal price)
    {
        var paymentResult = await _invoiceServices.PayAsync(invoiceId, price);
        var invoice = await _invoiceServices.GetInvoiceByIdAsync(invoiceId);
        

        if (invoice.PaymentStatus ==PaymentStatus.Payed)
        {
            return NotFound("Payment unsuccessful: The invoice payed Before");
        }

        if (invoice.TotalPrice != price)
        {
            return NotFound("Payment unsuccessful: Check the payment amount.");
        }
        if (paymentResult)
        {
            return Ok($"The invoice with Id {invoiceId} payed Successfully. ");
        }

        return NotFound("Payment unsuccessful");
    }

    [HttpDelete("DeleteInvoice/{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        var invoice = await _invoiceServices.GetInvoiceByIdAsync(id);
        if (invoice.PaymentStatus == PaymentStatus.Pending || invoice.PaymentStatus == PaymentStatus.Payed)
        {
            await _invoiceServices.DeleteInvoiceAsync(id);
            return Ok($"The invoice with ID {invoice} successfully Deleted!");
        }

        return NotFound("Check the payment Status!");
    }

    [HttpDelete("DeleteInvoiceProduct")]
    public async Task<IActionResult> DeleteInvoiceProduct(Guid invoiceId, Guid productId)
    {
        var deleted = await _invoiceServices.DeleteInvoiceProductAsync(invoiceId, productId);
        if (deleted)
        {
            return Ok($"Product with ID{productId} has successfully deleted from Invoice with Id{invoiceId}");
        }

        return NotFound($"There is no Product({productId}) inside the invoice ({invoiceId})!!");
    }
}