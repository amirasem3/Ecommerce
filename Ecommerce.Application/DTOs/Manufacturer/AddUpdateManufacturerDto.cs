using Ecommerce.Core.Entities.RelationEntities;

namespace Ecommerce.Application.DTOs.Manufacturer;

public class AddUpdateManufacturerDto
{
    public string Name { get; set; }
    public string OwnerName { get; set; }
    public string ManufacturerCountry { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public int Rate { get; set; }
    public DateTime EsatablishDate { get; set; }
    public bool Status { get; set; }
    
}