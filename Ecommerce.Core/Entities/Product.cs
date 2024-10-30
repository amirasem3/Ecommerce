using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Core.Entities;

public class Product
{
    public Guid Id { get; init; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$", ErrorMessage = "Name can only contain ")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name should be specified.")]
    public string Name { get; set; } = null!;
    
    [Column(TypeName = "decimal(15,2)")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:c}")]
    [Required(ErrorMessage = "Price must be Specified.")]
    public decimal Price { get; set; }
    
    [Column(TypeName = "decimal(15,2)")]
    [Required(ErrorMessage = "Inventory must be specified.")]
    public decimal Inventory { get; set; }
    
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date of production must be specified.")]
    public DateTime Dop { get; set; }
    
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date of expiration must be specified.")]
    public DateTime Doe { get; set; }

    public bool Status
    { 
        get ;
        set;
    }
    

    //Relation N-N with Invoice
    [JsonIgnore]
    public ICollection<ProductInvoice> Invoices { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Manufacturer> Manufacturers2 { get; set; } = null!;
    
}