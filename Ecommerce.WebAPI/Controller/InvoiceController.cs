using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;

namespace EcommerceSolution.Controller;

[Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceServices _invoiceServices;
    private readonly ILogger<InvoiceController> _logger;
    public InvoiceController(InvoiceServices invoiceServices, ILogger<InvoiceController> logger)
    {
        _invoiceServices = invoiceServices;
        _logger = logger;
    }

    [HttpGet("GetInvoiceById/{id}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            var invoice = await _invoiceServices.GetInvoiceByIdAsync(id);
            return Ok(invoice);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [id], retrievedData: e.Message, logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetInvoiceByIDNumber")]
    public async Task<IActionResult> GetInvoiceByIdNumber([FromQuery] string idNumber)
    {
        LoggerHelper.LogWithDetails(_logger,args: [idNumber]);
        try
        {
            var invoice = await _invoiceServices.GetInvoiceByIdentificationCodeAsync(idNumber);
            return Ok(invoice);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [idNumber], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoiceByOwnerName/{ownerName}")]
    public async Task<IActionResult> SearchInvoicesByOwnerName(string ownerName)
    {
        LoggerHelper.LogWithDetails(_logger,args: [ownerName]);
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByOwnerNameAsync(ownerName);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [ownerName], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoiceByOwnerLastName/{lastname}")]
    public async Task<IActionResult> SearchInvoicesByOwnerLastname(string lastname)
    {
        LoggerHelper.LogWithDetails(_logger,args: [lastname]);
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByOwnerFamilyNameAsync(lastname);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [lastname], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("SearchInvoicesByIssuerName/{issuerName}")]
    public async Task<IActionResult> SearchInvoicesByIssuerName(string issuerName)
    {
        LoggerHelper.LogWithDetails(_logger,args: [issuerName]);
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByIssuerNameAsync(issuerName);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [issuerName], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("PaymentStatusFilter/{paymentStatus}")]
    public async Task<IActionResult> PaymentStatusFiler(string paymentStatus)
    {
        LoggerHelper.LogWithDetails(_logger,args: [paymentStatus]);
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByPaymentStatusAsync(paymentStatus);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [paymentStatus], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);

            return NotFound(e.Message);
        }
    }

    [HttpGet("IssueDateFilter/{issueDate}")]
    public async Task<IActionResult> IssueDateFilter(DateTime issueDate)
    {
        LoggerHelper.LogWithDetails(_logger,args: [issueDate]);
        try
        {
            var invoices = await _invoiceServices.GetInvoiceByIssueDateAsync(issueDate);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [issueDate], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }

    [HttpGet("PaymentDataFilter/{paymentDate}")]
    public async Task<IActionResult> PaymentDateFilter(DateTime paymentDate)
    {
        LoggerHelper.LogWithDetails(_logger,args: [paymentDate]);
        try
        {
            var invoices = await _invoiceServices.GetInvoicesByPaymentDateAsync(paymentDate);
            return Ok(invoices);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [paymentDate], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);

            return NotFound(e.Message);
        }
    }

    [HttpGet("GetAllInvoices")]
    public async Task<IActionResult> GetAllInvoices()
    {
        LoggerHelper.LogWithDetails(_logger);
        try
        {
            var invoices = await _invoiceServices.GetAllInvoicesAsync();
            return Ok(invoices);
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);

            return NotFound(e.Message);
        }
    }

    [HttpPost("IssueNewInvoice")]
    [Consumes("application/json")]
    public async Task<IActionResult> IssueNewInvoice([FromBody] AddInvoiceDto newInvoice)
    {
        LoggerHelper.LogWithDetails(_logger,args: [newInvoice]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors", args: [newInvoice],
                retrievedData: ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList()!,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var invoice = await _invoiceServices.AddInvoiceAsync(newInvoice);
        return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
    }

    [HttpPost("AddInvoiceProducts")]
    public async Task<IActionResult> AddProductToInvoice([FromQuery] Guid invoiceId, [FromQuery] Guid productId,
        [FromQuery] int count)
    {
        LoggerHelper.LogWithDetails(_logger,args: [invoiceId, productId, count]);
        try
        {
            await _invoiceServices.AssignInvoiceProductAsync(invoiceId, productId, count);
            return Ok($"The {count}  products are added successfully.");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,args: [invoiceId, productId], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateInvoice/{id}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] UpdateInvoiceDto updateInvoiceDto)
    {
        LoggerHelper.LogWithDetails(_logger,args: [id, updateInvoiceDto]);
        if (!ModelState.IsValid)
        {
            LoggerHelper.LogWithDetails(_logger,"Binding Errors.", args: [updateInvoiceDto],
                retrievedData: ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList()!,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(ModelState["Invoice"]?.Errors.Select(e => e.ErrorMessage).ToList());
        }

        try
        {
            var updatedInvoice = await _invoiceServices.UpdateInvoiceAsync(id, updateInvoiceDto);
            return Ok(updatedInvoice);
        }
        catch (NullReferenceException e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [id, updateInvoiceDto], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
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
        LoggerHelper.LogWithDetails(_logger,args: [id]);
        try
        {
            await _invoiceServices.DeleteInvoiceAsync(id);
            return Ok($"The invoice with ID {id} successfully Deleted!");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Unexpected Errors", args: [id], retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteInvoiceProduct")]
    public async Task<IActionResult> DeleteInvoiceProduct(Guid invoiceId, Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,args: [invoiceId, productId]);
        try
        {
            await _invoiceServices.DeleteInvoiceProductAsync(invoiceId, productId);
            return Ok($"Product with ID{productId} has successfully deleted from Invoice with Id{invoiceId}");
        }
        catch (Exception e)
        {
            LoggerHelper.LogWithDetails(_logger,"Wrong productId or invoiceID", args: [invoiceId, productId],
                retrievedData: e.Message,
                logLevel: LoggerHelper.LogLevel.Error);
            return NotFound(e.Message);
        }
    }
}