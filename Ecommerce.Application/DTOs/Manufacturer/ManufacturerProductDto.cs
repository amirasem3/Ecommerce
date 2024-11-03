using System.Formats.Tar;

namespace Ecommerce.Application.DTOs.Manufacturer;

public class ManufacturerProductDto
{
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int Rate { get; set; }
}