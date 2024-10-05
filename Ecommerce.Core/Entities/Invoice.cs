﻿using Azure.Core.Pipeline;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Core.Entities;

public enum PayStat
{
    Pending,
    Payed,
    Cancelled
}
public class Invoice
{
    public Guid Id { get; set; }
    
    public string OwnerName { get; set; }
    
    public string OwnerFamilyName { get; set; }
    
    public string IdentificationCode { get; set; }
    
    public string IssuerName { get; set; }
    
    public DateTime IssueDate { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public decimal TotalPrice { get; set; }

    //Payment Status: Payed, Pending, Cencelled
    public string PaymentStatus { get; set; }
    
    public ICollection<ProductInvoice> Products { get; set; }
}