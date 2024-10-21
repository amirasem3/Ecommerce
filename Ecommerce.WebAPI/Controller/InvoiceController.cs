using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
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
        try
        {
            var invoice = await _invoiceServices.GetInvoiceByIdAsync(id);
            return Ok(invoice);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetInvoiceByIDNumber")]
    public async Task<IActionResult> GetInvoiceByIdNumber([FromQuery] string idNumber)
    {
        try
        {
            var invoice = await _invoiceServices.GetInvoiceByIdentificationCodeAsync(idNumber);
            return Ok(invoice);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoiceByOwnerName/{ownerName}")]
    public async Task<IActionResult> SearchInvoicesByOwnerName(string ownerName)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByOwnerNameAsync(ownerName);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoiceByOwnerLastName/{lastname}")]
    public async Task<IActionResult> SearchInvoicesByOwnerLastname(string lastname)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByOwnerFamilyNameAsync(lastname);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoicesByIssuerName/{issuerName}")]
    public async Task<IActionResult> SearchInvoicesByIssuerName(string issuerName)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByIssuerNameAsync(issuerName);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("PaymentStatusFilter/{paymentStatus}")]
    public async Task<IActionResult> PaymentStatusFiler(string paymentStatus)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByPaymentStatusAsync(paymentStatus);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("IssueDateFilter/{issueDate}")]
    public async Task<IActionResult> IssueDateFilter(DateTime issueDate)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoiceByIssueDateAsync(issueDate);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("PaymentDataFilter/{paymentDate}")]
    public async Task<IActionResult> PaymentDateFilter(DateTime paymentDate)
    {
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByPaymentDateAsync(paymentDate);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllInvoices")]
    public async Task<IActionResult> GetAllInvoices()
    {
        var invoices = await _invoiceServices.GetAllInvoicesAsync();
        return Ok(invoices);
    }

    [HttpPost("IssueNewInvoice")]
    [Consumes("application/json")]
    public async Task<IActionResult> IssueNewInvoice([FromBody] AddInvoiceDto newInvoice)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var invoice = await _invoiceServices.AddInvoiceAsync(newInvoice);
        return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
    }

    [HttpPost("AddInvoiceProducts")]
    public async Task<IActionResult> AddProductToInvoice([FromQuery] Guid invoiceId, [FromQuery] Guid productId,
        [FromQuery] int count)
    {
        try
        {
            await _invoiceServices.AssignInvoiceProductAsync(invoiceId, productId, count);
            return Ok($"The {count}  products are added successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateInvoice/{id}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] UpdateInvoiceDto updateInvoiceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList());
        }

        try
        {
            var updatedInvoice = await _invoiceServices.UpdateInvoiceAsync(id, updateInvoiceDto);
            return Ok(updatedInvoice);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("Pay")]
    public async Task<IActionResult> PayInvoice([FromQuery] Guid invoiceId, [FromQuery] decimal price)
    {
        try
        {
            await _invoiceServices.PayAsync(invoiceId, price);
            return Ok($"The invoice with Id {invoiceId} payed Successfully. ");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("DeleteInvoice/{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        try
        {
            await _invoiceServices.DeleteInvoiceAsync(id);
            return Ok($"The invoice with ID {id} successfully Deleted!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteInvoiceProduct")]
    public async Task<IActionResult> DeleteInvoiceProduct(Guid invoiceId, Guid productId)
    {
        try
        {
            await _invoiceServices.DeleteInvoiceProductAsync(invoiceId, productId);
            return Ok($"Product with ID{productId} has successfully deleted from Invoice with Id{invoiceId}");
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}