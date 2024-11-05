using System;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.DTOs;

public class InvoiceProductDto
{
    public Guid Id { get; set; }

    public string OwnerName { get; init; } = null!;

    public string OwnerFamilyName { get; init; } = null!;

    public string IdentificationCode { get; init; } = null!;
    
    public string IssuerName { get; init; } = null!;

    public DateTime IssueDate { get; init; }
    
    public DateTime? PaymentDate { get; init; }

    public decimal TotalPrice { get; init; }

    //Payment Status: Payed, Pending, Cencelled
    public PaymentStatus PaymentStatus { get; init; }
}