using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
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
    public const string InvoiceException = "Invoice Not Found";

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
        if (invoice == null)
        {
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

        return invoiceDto;
    }

    public async Task<InvoiceDto> GetInvoiceByIdentificationCodeAsync(string identificationCode)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdentificationCode(identificationCode);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

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
            Products = invoice.Products.Select(pi => new ProductInvoiceDto
            {
                Price = pi.Product.Price,
                Name = pi.Product.Name,
                Count = pi.Count
            }).ToList(),
        };
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerNameAsync(string ownerName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByOwnerName(ownerName);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByOwnerFamilyNameAsync(string familyName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByOwnerFamilyName(familyName);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByIssuerNameAsync(string issuerName)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByIssuerName(issuerName);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentStatusAsync(string paymentStatus)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByPaymentStatus(paymentStatus);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoiceByIssueDateAsync(DateTime issueDate)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByIssueDate(issueDate);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPaymentDateAsync(DateTime paymentDate)
    {
        var invoices = await _invoiceRepository.SearchInvoicesByPaymentDate(paymentDate);
        if (invoices == null)
        {
            throw new Exception(InvoiceException);
        }

        return invoices.Select(invoice => new InvoiceDto
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
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAllInvoices();

        return invoices.Select(i => new InvoiceDto
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
    }


    public async Task<InvoiceDto> AddInvoiceAsync(AddInvoiceDto addInvoiceDto)
    {
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
        await _invoiceRepository.AddInvoiceAsync(invoice);

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
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

        invoice.OwnerFirstName = updateInvoiceDto.OwnerName;
        invoice.OwnerLastName = updateInvoiceDto.OwnerFamilyName;
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
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }
        
        if (invoice.PaymentStatus != PaymentStatus.Pending && 
            invoice.PaymentStatus != PaymentStatus.Payed)
        {
            throw new ArgumentException("You cannot delete this invoice!");
        }

        await _invoiceRepository.DeleteInvoiceAsync(id);
        return true;
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (invoice == null)
        {
            throw new ArgumentException(InvoiceException);
        }

        if (product == null)
        {
            throw new ArgumentException(ProductService.ProductException);
        }

        return await _invoiceProductRepository.DeleteInvoiceProductAsync(invoiceId, productId);
    }

    public async Task<bool> PayAsync(Guid id, decimal price)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(id);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

        var totalPrice = await CalculateTotalPriceAsync(id);
        if (price > totalPrice || price < totalPrice)
        {
            throw new Exception("Payment unsuccessful: Check the payment amount.");
        }

        if (invoice.PaymentStatus == PaymentStatus.Payed)
        {
            throw new Exception("Payment unsuccessful: The invoice payed Before.");
        }

        foreach (var product in invoice.Products)
        {
            var newProduct = await _productRepository.GetProductByIdAsync(product.ProductId);
            newProduct.Inventory -= product.Count;
            await _productRepository.UpdateProductAsync(newProduct);
        }

        invoice.PaymentStatus = PaymentStatus.Payed;
        invoice.PaymentDate = DateTime.Now;
        await _invoiceRepository.UpdateInvoiceAsync(invoice);
        return true;
    }

    private async Task<decimal> CalculateTotalPriceAsync(Guid invoiceId)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

        decimal result = 0;
        foreach (var product in invoice.Products)
        {
            var productTemp = await _productRepository.GetProductByIdAsync(product.ProductId);
            if (productTemp == null)
            {
                throw new Exception(ProductService.ProductException);
            }

            result += product.Count * productTemp.Price;
        }

        return result;
    }

    public async Task AssignInvoiceProductAsync(Guid invoiceId, Guid productId, int count)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (invoice == null)
        {
            throw new ArgumentException(InvoiceException);
        }

        if (product == null)
        {
            throw new ArgumentException(ProductService.ProductException);
        }

        if (count > product.Inventory)
        {
            throw new ArgumentException($"The count number is more than {product.Name} inventory!");
        }

        var exists = await _invoiceProductRepository.InvoiceHaveTheProductAsync(invoiceId, productId);
        // Check if the user already has this role
        if (!exists)
        {
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
        else
        {
            var productInvoice = await _invoiceProductRepository.GetProductInvoiceAsync(invoiceId, productId);
            productInvoice.Count += count;
            await _invoiceProductRepository.UpdateProductInvoiceAsync(productInvoice);
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            await _invoiceRepository.UpdateInvoiceAsync(invoice);
        }
    }
}