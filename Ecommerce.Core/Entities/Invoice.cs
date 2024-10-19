using System.ComponentModel.DataAnnotations;
using Azure.Core.Pipeline;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Entities;

public enum PaymentStatus
{
    Pending,
    Payed,
    Cancelled
}

[Index(nameof(IdentificationCode), IsUnique = true)]
public class Invoice
{
    public Guid Id { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name should be specified.")]
    public string? OwnerFirstName { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Owner lastname should be specified.")]

    public string? OwnerLastName { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Owner lastname should be specified.")]
    public string? IdentificationCode { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Issuer name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Issuer name should be specified.")]
    public string? IssuerName { get; set; }


    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Issue date must be specified")]
    public DateTime IssueDate { get; set; }

    [DataType(DataType.Date)] public DateTime? PaymentDate { get; set; }


    public decimal TotalPrice { get; set; }

    //Payment Status: Payed, Pending, Cencelled
    public PaymentStatus PaymentStatus { get; set; }

    public ICollection<ProductInvoice> Products { get; set; }

    public ICollection<Product> Products2 { get; set; }
}