using System.Collections;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Application.Services;

public class InvoiceServices : IInvoiceServices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInvoiceProductRepository _invoiceProductRepository;

    public InvoiceServices(IInvoiceRepository invoiceRepository,
        IProductRepository productRepository, IInvoiceProductRepository invoiceProductRepository)
    {
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
        _invoiceProductRepository = invoiceProductRepository;
    }

    public async Task<InvoiceDto> GetInvoiceByIdAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        return new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        };
    }

    public async Task<InvoiceDto> GetInvoiceByIdentificationCodeAsync(string identificationCode)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdentificationCode(identificationCode);
        return new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        };
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerNameAsync(string ownerName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByOwnerName(ownerName);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerFamilyNameAsync(string familyName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByOwnerFamilyName(familyName);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByIssuerNameAsync(string issuerName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByIssuerName(issuerName);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentStatusAsync(string paymentStatus)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByPaymentStatus(paymentStatus);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoiceByIssueDateAsync(DateTime issueDate)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByIssueDate(issueDate);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentDate(DateTime paymentDate)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByPaymentDate(paymentDate);
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAllInvoices();
        return invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice
        });
    }


    public async Task<InvoiceDto> AddInvoiceAsync(AddUpdateInvoiceDto addInvoiceDto)
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            IdentificationCode = addInvoiceDto.IdentificationCode,
            IssueDate = addInvoiceDto.IssueDate,
            IssuerName = addInvoiceDto.IssuerName,
            OwnerName = addInvoiceDto.OwnerName,
            OwnerFamilyName = addInvoiceDto.OwnerFamilyName,
            PaymentDate = addInvoiceDto.PaymentDate,
            TotalPrice = addInvoiceDto.TotalPrice,
            PaymentStatus = addInvoiceDto.PaymentStatus,
            Products = []
        };
        await _invoiceRepository.AddInvoiceAsync(invoice);

        return new InvoiceDto
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice,
            ProductInvoices = invoice.Products
        };
    }

    public async Task<InvoiceDto> UpdateInvoiceAsync(Guid id, UpdateInvoiceDto updateInvoiceDto)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        invoice.OwnerName = updateInvoiceDto.OwnerName;
        invoice.OwnerFamilyName = updateInvoiceDto.OwnerFamilyName;
        invoice.IssuerName = invoice.IssuerName;
        invoice.IssueDate = invoice.IssueDate;
        invoice.PaymentStatus = invoice.PaymentStatus;
        invoice.PaymentDate = invoice.PaymentDate;
        invoice.TotalPrice = updateInvoiceDto.TotalPrice;
        invoice.IdentificationCode = invoice.IdentificationCode;
        await _invoiceRepository.UpdateInvoiceAsync(invoice);
        return new InvoiceDto()
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice,
            ProductInvoices = invoice.Products
        };
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        if (invoice != null)
        {
            await _invoiceRepository.DeleteInvoiceAsync(id);
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        var manufacturer = await _invoiceRepository.GetInvoiceById(invoiceId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (manufacturer == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (product == null)
        {
            throw new ArgumentException("Role not found.");
        }

        return await _invoiceProductRepository.DeleteInvoiceProductAsync(invoiceId, productId);
    }

    public async Task<bool> PayAsync(Guid id, decimal price)
    {
        var invoice = await _invoiceRepository.GetInvoiceProductAsync(id);
        var totalPrice = await CalculateTotalPriceAsync(id);
        if (price > totalPrice || price < totalPrice || invoice.PaymentStatus == "Payed")
        {
            return false;
        }

        foreach (var product in invoice.Products)
        {
            var newProduct = await _productRepository.GetProductByIdAsync(product.ProductId);
            newProduct.Inventory -= product.Count;
            await _productRepository.UpdateProductAsync(newProduct);
        }

        invoice.PaymentStatus = "Payed";
        invoice.PaymentDate = DateTime.Today;
        await _invoiceRepository.UpdateInvoiceAsync(invoice);
        return true;
    }

    public async Task<decimal> CalculateTotalPriceAsync(Guid invoiceId)
    {
        var invoice = await _invoiceRepository.GetInvoiceProductAsync(invoiceId);
        decimal result = 0;
        foreach (var product in invoice.Products)
        {
            var price = await _productRepository.GetProductByIdAsync(product.ProductId);
            result += product.Count * price.Price;
        }

        return result;
    }

    public async Task<InvoiceDto> GetInvoiceProductAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetInvoiceProductAsync(id);

        return new InvoiceDto()
        {
            Id = invoice.Id,
            OwnerName = invoice.OwnerName,
            IdentificationCode = invoice.IdentificationCode,
            OwnerFamilyName = invoice.OwnerFamilyName,
            IssuerName = invoice.IssuerName,
            IssueDate = invoice.IssueDate,
            PaymentDate = invoice.PaymentDate,
            PaymentStatus = invoice.PaymentStatus,
            TotalPrice = invoice.TotalPrice,
            ProductInvoices = invoice.Products
        };
    }

    public async Task AssignInvoiceProductAsync(Guid invoiceId, Guid productId, int count)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (invoice == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (product == null)
        {
            throw new ArgumentException("Role not found.");
        }

        if (count > product.Inventory)
        {
            throw new ArgumentException($"The count number is more than {product.Name} inventory!");
        }

        var exists = await _invoiceProductRepository.InvoiceHaveTheProductAsync(invoiceId, productId);
        // Check if the user already has this role
        if (!exists)
        {
            invoice.PaymentStatus = "Pending";
            await _invoiceRepository.UpdateInvoiceAsync(invoice);
            var invoiceProduct = new ProductInvoice
            {
                InvoiceId = invoiceId,
                ProductId = productId,
                Count = count,
            };


            await _invoiceProductRepository.AddInvoiceProductAsync(invoiceProduct);
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            await _invoiceRepository.UpdateInvoiceAsync(invoice);
        }
    }
}