using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Repositories;
using Exception = System.Exception;

namespace Ecommerce.Application.Services;

public class InvoiceServices
{
    public const string InvoiceException = "Invoice Not Found";
    private readonly UnitOfWork _unitOfWork;

    public InvoiceServices(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to get a invoice by ID", args: [id]);
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice with this ID", args: [id],
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

        LoggerHelper.LogWithDetails("Invoice Found", args: [id], retrievedData: invoiceDto);

        return invoiceDto;
    }

    public async Task<InvoiceDto> GetInvoiceByIdentificationCodeAsync(string identificationCode)
    {
        LoggerHelper.LogWithDetails("Attempt to get a invoice by identification number", args: [identificationCode]);
        var invoice = await _unitOfWork.InvoiceRepository.GetByUniquePropertyAsync(uniqueProperty: "IdentificationCode",
            uniquePropertyValue: identificationCode, includeProperties: "Products,Products.Product");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice with this identification number",
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [identificationCode], retrievedData: invoiceDto);
        return invoiceDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerNameAsync(string ownerName)
    {
        LoggerHelper.LogWithDetails("Attempt to get a invoice by owner name", args: [ownerName]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerFirstName.Contains(ownerName),
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [ownerName], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [ownerName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerFamilyNameAsync(string lastName)
    {
        LoggerHelper.LogWithDetails("Attempt to get a invoice by owner lastname", args: [lastName]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerLastName.Contains(lastName),
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [lastName], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [lastName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByIssuerNameAsync(string issuerName)
    {
        LoggerHelper.LogWithDetails("Attempt to get a invoice by issuer name", args: [issuerName]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.IssuerName.Contains(issuerName), includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [issuerName], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [issuerName], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentStatusAsync(string paymentStatus)
    {
        LoggerHelper.LogWithDetails("Attempt to filter invoices by payment status", args: [paymentStatus]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentStatus == (PaymentStatus)Enum.Parse(typeof(PaymentStatus), paymentStatus)
            , includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [paymentStatus], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [paymentStatus], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoiceByIssueDateAsync(DateTime issueDate)
    {
        LoggerHelper.LogWithDetails("Attempt to filter by issue date", args: [issueDate]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.IssueDate == issueDate, includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [issueDate], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [issueDate], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentDateAsync(DateTime paymentDate)
    {
        LoggerHelper.LogWithDetails("Attempt to filter by payment date", args: [paymentDate]);
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentDate == paymentDate, includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(args: [paymentDate], retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", args: [paymentDate], retrievedData: invoicesDto);
        return invoicesDto;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        LoggerHelper.LogWithDetails("Attempts to get all invoices");
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            includeProperties: "Products,Products.Product");
        if (invoices == null)
        {
            LoggerHelper.LogWithDetails(retrievedData: InvoiceException
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
        LoggerHelper.LogWithDetails("Invoice Found", retrievedData: invoicesDto);

        return invoicesDto;
    }


    public async Task<InvoiceDto> AddInvoiceAsync(AddInvoiceDto addInvoiceDto)
    {
        LoggerHelper.LogWithDetails("Attempt to add a new invoice", args: [addInvoiceDto]);
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
        await _unitOfWork.InvoiceRepository.InsertAsync(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Insert", retrievedData: invoice);
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
        LoggerHelper.LogWithDetails("Attempt to update a invoice", args: [id, updateInvoiceDto]);
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails(retrievedData: InvoiceException
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails("Target Invoice Found", args: [id], retrievedData: invoice);

        invoice.OwnerFirstName = updateInvoiceDto.OwnerName;
        invoice.OwnerLastName = updateInvoiceDto.OwnerFamilyName;
        invoice.IssuerName = invoice.IssuerName;
        invoice.IssueDate = invoice.IssueDate;
        invoice.PaymentStatus = invoice.PaymentStatus;
        invoice.PaymentDate = invoice.PaymentDate;
        invoice.TotalPrice = updateInvoiceDto.TotalPrice;
        invoice.IdentificationCode = invoice.IdentificationCode;

        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();

        var invoiceDto = new InvoiceDto()
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
        LoggerHelper.LogWithDetails("Successful update", args: [id, updateInvoiceDto], retrievedData: invoiceDto);
        return invoiceDto;
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        LoggerHelper.LogWithDetails("Attempt to Delete an invoice.", args: [id]);
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("Theres is no invoice with this ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails("Target Invoice Found", args: [id], retrievedData: invoice);
        if (invoice.PaymentStatus == PaymentStatus.Payed)
        {
            LoggerHelper.LogWithDetails("Cannot delete an Payed invoice.", args: [id],
                retrievedData: invoice.PaymentStatus, logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException("Cannot delete an Payed invoice.");
        }

        await _unitOfWork.InvoiceRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Delete", args: [id], retrievedData: invoice);
        return true;
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        LoggerHelper.LogWithDetails("Attempt to delete a product from the invoice's products");
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("Theres is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(InvoiceException);
        }

        LoggerHelper.LogWithDetails("Target Invoice Found", args: [invoiceId], retrievedData: invoice);
        if (invoice.PaymentStatus == PaymentStatus.Payed)
        {
            LoggerHelper.LogWithDetails("You cannot modify an payed invoice's products",
                retrievedData: invoice.PaymentStatus, logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("You cannot modify an payed invoice's products");
        }

        var targetProductInvoice =
            invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        invoice.Products.Remove(targetProductInvoice!);
        if (targetProductInvoice!.Count >1)
        {
            targetProductInvoice.Count -= 1;
            invoice.TotalPrice -= product.Price;
            invoice.Products.Add(targetProductInvoice);
            _unitOfWork.InvoiceRepository.Update(invoice);
            await _unitOfWork.SaveAsync();
            return true;
        }

        invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("The product successfully removed from invoice's product",
            args: [invoiceId, productId], retrievedData: invoice.Products);
        return true;
    }

    public async Task<bool> PayAsync(Guid id, decimal price)
    {
        LoggerHelper.LogWithDetails("Attempt to pay an invoice");
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice with this invoice ID", args: [id],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails("Target Invoice Found.", args: [id], retrievedData: invoice);
        var totalPrice = invoice.TotalPrice;
        if (price > totalPrice || price < totalPrice)
        {
            LoggerHelper.LogWithDetails("Payment unsuccessful: Check the payment amount.", args:
                [price, invoice.TotalPrice]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Payment unsuccessful: Check the payment amount.");
        }

        if (invoice.PaymentStatus == PaymentStatus.Payed)
        {
            LoggerHelper.LogWithDetails("Payment unsuccessful: The invoice is payed before.", args:
                [invoice.PaymentStatus]
                , logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception("Payment unsuccessful: The invoice payed Before.");
        }

        LoggerHelper.LogWithDetails("Increase the invoice's products' inventory");
        foreach (var product in invoice.Products)
        {
            var newProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
            newProduct.Inventory -= product.Count;
            _unitOfWork.ProductRepository.Update(newProduct);
            await _unitOfWork.SaveAsync();
            // await _productRepository.UpdateProductAsync(newProduct);
        }

        invoice.PaymentStatus = PaymentStatus.Payed;
        LoggerHelper.LogWithDetails("Invoice payment status successfully changed.",
            retrievedData: invoice.PaymentStatus);
        invoice.PaymentDate = DateTime.Now;
        LoggerHelper.LogWithDetails("Invoice payment date successfully set.", retrievedData: invoice.PaymentDate);
        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        LoggerHelper.LogWithDetails("Successful Payment.", args: [id, price], retrievedData: invoice);
        return true;
    }

    private async Task<decimal> CalculateTotalPriceAsync(Guid invoiceId)
    {
        LoggerHelper.LogWithDetails("Attempt to calculate an invoice's total price");
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new Exception(InvoiceException);
        }

        LoggerHelper.LogWithDetails("Target Invoice Found.", args: [invoiceId], retrievedData: invoice);

        decimal result = 0;
        foreach (var product in invoice.Products)
        {
            var productTemp = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
            result += product.Count * productTemp.Price;
        }

        LoggerHelper.LogWithDetails($"Total price for the {invoice.OwnerLastName}'s invoice successfully calculated.",
            retrievedData: result);
        return result;
    }

    public async Task AssignInvoiceProductAsync(Guid invoiceId, Guid productId, int count)
    {
        LoggerHelper.LogWithDetails("Attempt to add a product to a invoice", args: [productId, invoiceId, count]);
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        if (invoice == null)
        {
            LoggerHelper.LogWithDetails("There is no invoice with this invoice ID", args: [invoiceId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(InvoiceException);
        }

        if (invoice.PaymentStatus == PaymentStatus.Payed)
        {
            throw new Exception("You cannot modify an Payed invoice's products");
        }

        LoggerHelper.LogWithDetails("Target Invoice Found.", args: [invoiceId], retrievedData: invoice);

        if (product == null)
        {
            LoggerHelper.LogWithDetails("There is no product with this product ID", args: [productId],
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException(ProductService.ProductException);
        }

        LoggerHelper.LogWithDetails("Target Product Found.", args: [productId], retrievedData: product);

        if (count > product.Inventory)
        {
            LoggerHelper.LogWithDetails($"The count number is more than {product.Name}'s inventory!",
                logLevel: LoggerHelper.LogLevel.Error);
            throw new ArgumentException($"The count number is more than {product.Name}'s inventory!");
        }

        var exists = invoice.Products.Any(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        LoggerHelper.LogWithDetails("Check if the product exist in the invoice products", args: [productId, invoiceId],
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
            LoggerHelper.LogWithDetails("New product added to the invoice's products");
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            LoggerHelper.LogWithDetails("Invoice's total price changed successfully.",
                retrievedData: invoice.TotalPrice);
            invoice.PaymentStatus = PaymentStatus.Pending;
            LoggerHelper.LogWithDetails("Invoice's payment status changed successfully.",
                retrievedData: invoice.PaymentStatus);
            _unitOfWork.InvoiceRepository.Update(invoice);
            LoggerHelper.LogWithDetails("Invoice's products updated successfully.", retrievedData: invoice.Products);
            await _unitOfWork.SaveAsync();
        }
        else
        {
            LoggerHelper.LogWithDetails("The product exists in the invoice's products");
            var targetProductInvoice =
                invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
            LoggerHelper.LogWithDetails("Remove the product with old count from the invoice's products.");
            invoice.Products.Remove(targetProductInvoice!);
            targetProductInvoice!.Count += count;
            LoggerHelper.LogWithDetails("Increase the count of the product in the invoice.",
                retrievedData: targetProductInvoice.Count);
            invoice.Products.Add(targetProductInvoice);
            LoggerHelper.LogWithDetails("Add the product with new count to the invoice's products");
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            LoggerHelper.LogWithDetails("Calculate the invoice's total price", retrievedData: invoice.TotalPrice);
            invoice.PaymentStatus = PaymentStatus.Pending;
            LoggerHelper.LogWithDetails("Invoice's payment status changed successfully.",
                retrievedData: invoice.PaymentStatus);
            _unitOfWork.InvoiceRepository.Update(invoice);
            LoggerHelper.LogWithDetails("Invoice's products updated successfully.", retrievedData: invoice.Products);
            await _unitOfWork.SaveAsync();
        }
    }
}