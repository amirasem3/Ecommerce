using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Serilog;
using Exception = System.Exception;
using ILogger = Serilog.ILogger;

namespace Ecommerce.Application.Services;

public class InvoiceServices
{
    public const string InvoiceException = "Invoice Not Found";
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<InvoiceServices> _logger;

    public InvoiceServices(UnitOfWork unitOfWork, ILogger<InvoiceServices> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a invoice by ID", args: [id]);
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(id, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoiceDto = new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = await CalculateTotalPriceAsync(id),
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        };

        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [id], retrievedData: invoiceDto);

        return invoiceDto;
    }

    public async Task<InvoiceDto> GetInvoiceByIdentificationCodeAsync(string identificationCode)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a invoice by identification number", args: [identificationCode]);
        var invoice = await _unitOfWork.invoiceRepository.GetByUniquePropertyAsync(uniqueProperty: "IdentificationCode",
            uniquePropertyValue: identificationCode, includeProperties: "Products,Products.Product");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice with this identification number",
                args: [identificationCode],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoiceDto = new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        };
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [identificationCode], retrievedData: invoiceDto);
        return invoiceDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerNameAsync(string ownerName)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a invoice by owner name", args: [ownerName]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerFirstName.Contains(ownerName),
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [ownerName], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [ownerName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerFamilyNameAsync(string lastName)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a invoice by owner lastname", args: [lastName]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerLastName.Contains(lastName),
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [lastName], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [lastName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByIssuerNameAsync(string issuerName)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to get a invoice by issuer name", args: [issuerName]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.IssuerName.Contains(issuerName), includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [issuerName], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [issuerName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentStatusAsync(string paymentStatus)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to filter invoices by payment status", args: [paymentStatus]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentStatus == (PaymentStatus)Enum.Parse(typeof(PaymentStatus), paymentStatus)
            , includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [paymentStatus], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [paymentStatus], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoiceByIssueDateAsync(DateTime issueDate)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to filter by issue date", args: [issueDate]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.IssueDate == issueDate, includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [issueDate], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [issueDate], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentDateAsync(DateTime paymentDate)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to filter by payment date", args: [paymentDate]);
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentDate == paymentDate, includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,args: [paymentDate], retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", args: [paymentDate], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        LoggerHelper.LogWithDetails(_logger,"Attempts to get all invoices");
        var invoices = await _unitOfWork.invoiceRepository.GetAsync(
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(_logger,retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        var invoicesDto = invoices.Select(i => new InvoiceDto
        {
            Id = i.Id,
            OwnerName = i.OwnerFirstName,
            IdentificationCode = i.IdentificationCode,
            OwnerFamilyName = i.OwnerLastName,
            IssuerName = i.IssuerName,
            IssueDate = i.IssueDate,
            PaymentDate = i.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), i.PaymentStatus)!,
            TotalPrice = i.TotalPrice,
            Products = i.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        });
        LoggerHelper.LogWithDetails(_logger,"Invoice Found", retrievedData: invoicesDto);

        return invoicesDto;
    }


    public async Task<InvoiceDto> AddInvoiceAsync(AddInvoiceDto addInvoiceDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to add a new invoice", args: [addInvoiceDto]);
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            IdentificationCode = addInvoiceDto.IdentificationCode,
            IssueDate = addInvoiceDto.IssueDate,
            IssuerName = addInvoiceDto.IssuerName,
            OwnerFirstName = addInvoiceDto.OwnerName,
            OwnerLastName = addInvoiceDto.OwnerFamilyName,
            PaymentDate = null,
            TotalPrice = addInvoiceDto.TotalPrice,
            PaymentStatus = PaymentStatus.Pending,
            Products = []
        };
        await _unitOfWork.invoiceRepository.InsertAsync(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Insert", retrievedData: invoice);
        return new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = [],
        };
    }

    public async Task<InvoiceDto> UpdateInvoiceAsync(Guid id, UpdateInvoiceDto updateInvoiceDto)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to update a invoice", args: [id, updateInvoiceDto]);
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(id, "Products,Products.Product");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        if (invoice.IsPayed())
        {
            LoggerHelper.LogWithDetails(_logger,"You cannot modify an payed invoice", retrievedData: invoice.IsPayed(),
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("You cannot modify an payed invoice");
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found", args: [id], retrievedData: invoice);

        invoice.OwnerFirstName = updateInvoiceDto.OwnerName;
        invoice.OwnerLastName = updateInvoiceDto.OwnerFamilyName;
        invoice.IssuerName = invoice.IssuerName;
        invoice.IssueDate = invoice.IssueDate;
        invoice.PaymentStatus = invoice.PaymentStatus;
        invoice.PaymentDate = invoice.PaymentDate;
        invoice.TotalPrice = invoice.CheckProducts() ? await CalculateTotalPriceAsync(id) : updateInvoiceDto.TotalPrice;
        invoice.IdentificationCode = invoice.IdentificationCode;

        // _unitOfWork.invoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();

        var invoiceDto = new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerFirstName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerLastName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = Enum.GetName(typeof(PaymentStatus), invoice.PaymentStatus)!,
            TotalPrice = invoice.TotalPrice,
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        };
        LoggerHelper.LogWithDetails(_logger,"Successful update", args: [id, updateInvoiceDto], retrievedData: invoiceDto);
        return invoiceDto;
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to Delete an invoice.", args: [id]);
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Theres is no invoice with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found", args: [id], retrievedData: invoice);
        if (invoice.IsPayed())
        {
            LoggerHelper.LogWithDetails(_logger,"Cannot delete an Payed invoice.", args: [id],
                retrievedData: invoice.PaymentStatus, logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException("Cannot delete an Payed invoice.");
        }

        // await _unitOfWork.invoiceRepository.DeleteByIdAsync(id);
        await _unitOfWork.invoiceRepository.Delete(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Delete", args: [id], retrievedData: invoice);
        return true;
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to delete a product from the invoice's products");
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.productRepository.GetByIdAsync(productId);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"Theres is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(InvoiceException);
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found", args: [invoiceId], retrievedData: invoice);
        if (invoice.IsPayed())
        {
            LoggerHelper.LogWithDetails(_logger,"You cannot modify an payed invoice's products",
                retrievedData: invoice.PaymentStatus, logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("You cannot modify an payed invoice's products");
        }

        var targetProductInvoice =
            invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        invoice.Products.Remove(targetProductInvoice!);
        if (targetProductInvoice!.CheckCount())
        {
            targetProductInvoice.Count -= 1;
            invoice.TotalPrice -= product.Price;
            invoice.Products.Add(targetProductInvoice);
            _unitOfWork.invoiceRepository.Update(invoice);
            await _unitOfWork.SaveAsync();
            return true;
        }

        invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
        // _unitOfWork.invoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"The product successfully removed from invoice's product",
            args: [invoiceId, productId], retrievedData: invoice.Products);
        return true;
    }

    public async Task<bool> PayAsync(Guid id, decimal price)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to pay an invoice");
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(id, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice with this invoice ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found.", args: [id], retrievedData: invoice);
        var totalPrice = invoice.TotalPrice;
        if (invoice.CheckPrice(price))
        {
            LoggerHelper.LogWithDetails(_logger,"Payment unsuccessful: Check the payment amount.", args:
                [price, invoice.TotalPrice]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Payment unsuccessful: Check the payment amount.");
        }

        if (invoice.IsPayed())
        {
            LoggerHelper.LogWithDetails(_logger,"Payment unsuccessful: The invoice is payed before.", args:
                [invoice.PaymentStatus]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Payment unsuccessful: The invoice payed Before.");
        }

        LoggerHelper.LogWithDetails(_logger,"Increase the invoice's products' inventory");
        foreach (var product in invoice.Products)
        {
            var newProduct = await _unitOfWork.productRepository.GetByIdAsync(product.ProductId);
            newProduct.Inventory -= product.Count;
            // _unitOfWork.productRepository.Update(newProduct);
            // await _unitOfWork.SaveAsync();
            // await _productRepository.UpdateProductAsync(newProduct);
        }

        invoice.PaymentStatus = PaymentStatus.Payed;
        LoggerHelper.LogWithDetails(_logger,"Invoice payment status successfully changed.",
            retrievedData: invoice.PaymentStatus);
        invoice.PaymentDate = DateTime.Now;
        LoggerHelper.LogWithDetails(_logger,"Invoice payment date successfully set.", retrievedData: invoice.PaymentDate);
        // _unitOfWork.invoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails(_logger,"Successful Payment.", args: [id, price], retrievedData: invoice);
        return true;
    }

    private async Task<decimal> CalculateTotalPriceAsync(Guid invoiceId)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to calculate an invoice's total price");
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(invoiceId, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found.", args: [invoiceId], retrievedData: invoice);

        decimal result = 0;
        foreach (var product in invoice.Products)
        {
            var productTemp = await _unitOfWork.productRepository.GetByIdAsync(product.ProductId);
            result += product.Count * productTemp.Price;
        }

        LoggerHelper.LogWithDetails(_logger,$"Total price for the {invoice.OwnerLastName}'s invoice successfully calculated.",
            retrievedData: result);
        return result;
    }

    public async Task AssignInvoiceProductAsync(Guid invoiceId, Guid productId, int count)
    {
        LoggerHelper.LogWithDetails(_logger,"Attempt to add a product to a invoice", args: [productId, invoiceId, count]);
        var invoice = await _unitOfWork.invoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.productRepository.GetByIdAsync(productId);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(InvoiceException);
        }

        if (invoice.IsPayed())
        {
            throw new Exception("You cannot modify an Payed invoice's products");
        }

        LoggerHelper.LogWithDetails(_logger,"Target Invoice Found.", args: [invoiceId], retrievedData: invoice);

        if (product == null)
        {
            LoggerHelper.LogWithDetails(_logger,"There is no product with this product ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        LoggerHelper.LogWithDetails(_logger,"Target Product Found.", args: [productId], retrievedData: product);

        if (product.CheckInventory(count))
        {
            LoggerHelper.LogWithDetails(_logger,$"The count number is more than {product.Name}'s inventory!",
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException($"The count number is more than {product.Name}'s inventory!");
        }

        var exists = invoice.Products.Any(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        LoggerHelper.LogWithDetails(_logger,"Check if the product exist in the invoice products", args: [productId, invoiceId],
            retrievedData: exists);
        if (!exists)
        {
            var invoiceProduct = new ProductInvoice
            {
                InvoiceId = invoiceId,
                ProductId = productId,
                Count = count,
            };
            invoice.Products.Add(invoiceProduct);
            LoggerHelper.LogWithDetails(_logger,"New product added to the invoice's products");
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            LoggerHelper.LogWithDetails(_logger,"Invoice's total price changed successfully.",
                retrievedData: invoice.TotalPrice);
            invoice.PaymentStatus = PaymentStatus.Pending;
            LoggerHelper.LogWithDetails(_logger,"Invoice's payment status changed successfully.",
                retrievedData: invoice.PaymentStatus);
            // _unitOfWork.invoiceRepository.Update(invoice);
            LoggerHelper.LogWithDetails(_logger,"Invoice's products updated successfully.", retrievedData: invoice.Products);
            
        }
        else
        {
            LoggerHelper.LogWithDetails(_logger,"The product exists in the invoice's products");
            var targetProductInvoice =
                invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
            LoggerHelper.LogWithDetails(_logger,"Remove the product with old count from the invoice's products.");
            invoice.Products.Remove(targetProductInvoice!);
            targetProductInvoice!.Count += count;
            LoggerHelper.LogWithDetails(_logger,"Increase the count of the product in the invoice.",
                retrievedData: targetProductInvoice.Count);
            invoice.Products.Add(targetProductInvoice);
            LoggerHelper.LogWithDetails(_logger,"Add the product with new count to the invoice's products");
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            LoggerHelper.LogWithDetails(_logger,"Calculate the invoice's total price", retrievedData: invoice.TotalPrice);
            invoice.PaymentStatus = PaymentStatus.Pending;
            LoggerHelper.LogWithDetails(_logger,"Invoice's payment status changed successfully.",
                retrievedData: invoice.PaymentStatus);
            // _unitOfWork.invoiceRepository.Update(invoice);
            LoggerHelper.LogWithDetails(_logger,"Invoice's products updated successfully.", retrievedData: invoice.Products);
            // await _unitOfWork.SaveAsync();
        }
        await _unitOfWork.SaveAsync();
    }
}