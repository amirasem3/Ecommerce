using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class Category
{
    public Guid Id { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$",
        ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name must be specified.")]
    public string Name { get; set; } = null!;
    
    
    public bool Type { get; set; }
    public Guid ParentCategoryId { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}