using System.Text.Json.Serialization;
using Ecommerce.Application.DTOs.Manufacturer;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;

namespace Ecommerce.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    
    public decimal Inventory { get; set; }
    
    public DateTime Dop { get; set; }
    
    public DateTime Doe { get; set; }
    
    public bool Status { get; set; }

    [JsonIgnore]
    public ICollection<ManufacturerProductDto> Manufacturer { get; set; } = null!;

    public ICollection<ProductInvoice> ProductInvoices { get; set; } = null!;
    
    
    
    
    
    
}