

using System;

namespace Ecommerce.Application.DTOs.Manufacturer;

public class AddUpdateManufacturerDto
{
    public string Name { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public string ManufacturerCountry { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int Rate { get; set; }
    public DateTime EstablishDate { get; set; }
    public bool Status { get; set; }
    
}