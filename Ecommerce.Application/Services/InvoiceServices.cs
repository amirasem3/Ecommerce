using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Invoice;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Infrastructure.Repositories;

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
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
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
        var invoice = await _unitOfWork.InvoiceRepository.GetByUniquePropertyAsync(uniqueProperty: "IdentificationCode",
            uniquePropertyValue: identificationCode, includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerFirstName.Contains(ownerName),
            includeProperties: "Products,Products.Product");

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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.OwnerLastName.Contains(familyName),
            includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.IssuerName.Contains(issuerName), includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentStatus == (PaymentStatus)Enum.Parse(typeof(PaymentStatus), paymentStatus)
            , includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.IssueDate == issueDate, includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            filter: invoice => invoice.PaymentDate == paymentDate, includeProperties: "Products,Products.Product");
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
        var invoices = await _unitOfWork.InvoiceRepository.GetAsync(
            includeProperties: "Products,Products.Product");

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
        await _unitOfWork.InvoiceRepository.InsertAsync(invoice);
        await _unitOfWork.SaveAsync();
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
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
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

        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
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
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

        if (invoice.PaymentStatus != PaymentStatus.Pending &&
            invoice.PaymentStatus != PaymentStatus.Payed)
        {
            throw new ArgumentException("You cannot delete this invoice!");
        }

        await _unitOfWork.InvoiceRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteInvoiceProductAsync(Guid invoiceId, Guid productId)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        if (invoice == null)
        {
            throw new ArgumentException(InvoiceException);
        }

        if (product == null)
        {
            throw new ArgumentException(ProductService.ProductException);
        }

        var targetProductInvoice =
            invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        invoice.Products.Remove(targetProductInvoice!);
        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        return true;
        // return await _invoiceProductRepository.DeleteInvoiceProductAsync(invoiceId, productId);
    }

    public async Task<bool> PayAsync(Guid id, decimal price)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id, "Products");
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
            var newProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
            newProduct.Inventory -= product.Count;
            _unitOfWork.ProductRepository.Update(newProduct);
            await _unitOfWork.SaveAsync();
            // await _productRepository.UpdateProductAsync(newProduct);
        }

        invoice.PaymentStatus = PaymentStatus.Payed;
        invoice.PaymentDate = DateTime.Now;
        _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.SaveAsync();
        return true;
    }

    private async Task<decimal> CalculateTotalPriceAsync(Guid invoiceId)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        if (invoice == null)
        {
            throw new Exception(InvoiceException);
        }

        decimal result = 0;
        foreach (var product in invoice.Products)
        {
            var productTemp = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
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
        var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, "Products");
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
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

        var exists = invoice.Products.Any(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
        if (!exists)
        {
            var invoiceProduct = new ProductInvoice
            {
                InvoiceId = invoiceId,
                ProductId = productId,
                Count = count,
            };
            invoice.Products.Add(invoiceProduct);
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            invoice.PaymentStatus = PaymentStatus.Pending;
            _unitOfWork.InvoiceRepository.Update(invoice);
            await _unitOfWork.SaveAsync();
        }
        else
        {
            var targetProductInvoice =
                invoice.Products.FirstOrDefault(pi => pi.InvoiceId == invoiceId && pi.ProductId == productId);
            invoice.Products.Remove(targetProductInvoice!);
            targetProductInvoice!.Count += count;
            invoice.Products.Add(targetProductInvoice);
            invoice.TotalPrice = await CalculateTotalPriceAsync(invoiceId);
            invoice.PaymentStatus = PaymentStatus.Pending;
            _unitOfWork.InvoiceRepository.Update(invoice);
            await _unitOfWork.SaveAsync();
        }
    }
}