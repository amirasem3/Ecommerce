using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email),IsUnique = true)]
public class User
{
    public Guid Id { get; set; }

    [RegularExpression(@"^[a-zA-Z''\s]+$",
        ErrorMessage = "Invalid Characters!")]
    [Required(ErrorMessage = "Firstname must be specified.")]
    [MaxLength(40,ErrorMessage = "Firstname cannot exceed 40 characters.")]
    public string? FirstName { get; set; }

    [RegularExpression(@"^[a-zA-Z''\s]+$",
        ErrorMessage = "Invalid Characters!")]
    [Required(ErrorMessage = "Lastname must be specified.")]
    [MaxLength(40,ErrorMessage = "Lastname cannot exceed 40 characters.")]
    public string? LastName { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''\s]+$",
        ErrorMessage = "Invalid Characters!")]
    [Required(ErrorMessage = "Username must be specified.")]
    [MaxLength(30,ErrorMessage = "Username cannot exceed 30 characters.")]
    public string? Username { get; set; }
    
    [MaxLength(90,ErrorMessage = "Passwordhash cannot exceed 30 characters.")]
    public string? PasswordHash { get; set; }

    [EmailAddress]
    [MaxLength(30,ErrorMessage = "Email cannot exceed 30 characters.")]
    public string? Email { get; set; }
    

    [Phone]
    [MaxLength(20,ErrorMessage = "PhoneNumber cannot exceed 30 characters.")]
    public string? PhoneNumber { get; set; }
    public Guid RoleId { get; set; }
}