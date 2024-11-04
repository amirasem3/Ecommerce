using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Address), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class Manufacturer
{
    public Guid Id { get; init; }

    [RegularExpression(@"^[a-zA-Z''\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name should be specified.")]
    public string Name { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z''\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Owner Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Owner Name should be specified.")]
    public string OwnerName { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z''\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Manufacturer Country cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name should be specified.")]
    public string ManufacturerCountry { get; set; } = null!;

    [EmailAddress]
    [MaxLength(20, ErrorMessage = "Email address cannot exceed 20 characters.")]
    [Required(ErrorMessage = "Email address must be specified.")]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z0-9''\s]+$", ErrorMessage = "Invalid Characters!")]
    [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    [Required(ErrorMessage = "Address should be specified.")]
    public string Address { get; set; } = null!;

    [Phone]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 100 characters.")]
    [Required(ErrorMessage = "Phone number should be specified.")]
    public string PhoneNumber { get; set; } = null!;

    public int Rate { get; set; }

    [DataType(DataType.Date)] [Required] public DateTime EstablishDate { get; set; }
    public bool Status { get; set; }

    //Relation N-N with Product
    [JsonIgnore] public ICollection<Product> Products2 { get; set; } = null!;

    public bool CheckProducts()
    {
        return Products2.Count != 0;
    }
}