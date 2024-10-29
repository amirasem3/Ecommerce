using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Entities;
[Index(nameof(Name), IsUnique = true)]
public class Role
{
    public Guid Id { get; set; }

    [RegularExpression(@"^[a-zA-Z''\s]+$",
        ErrorMessage = "Characters are not allowed.")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name should be specified.")]
    public string Name { get; set; } = null!;


    public override string ToString()
    {
        return $"\n\t Id : {Id}" +
               $"\n\t Name : {Name}";
    }
}