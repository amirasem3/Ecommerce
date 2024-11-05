using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecommerce.Core.Entities;

public class Category
{
    public Guid Id { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9''-'\s]+$",
        ErrorMessage = "Invalid Characters!")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    [Required(ErrorMessage = "Name must be specified.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Type must be specified.")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    public string TypeString { get; set; } = null!;

    public Guid? ParentCategoryId { get; set; }

    [JsonIgnore] public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    public bool IsParent()
    {
        return TypeString == "Parent";
    }

    public bool IsParentChild()
    {
        return TypeString == "Parent" && ParentCategoryId != Guid.Empty;
    }

    public bool ParentInParent()
    {
        return IsParentChild() || (!IsParent() && !IsParentChild());
    }

    public bool IsChild()
    {
        return IsParent() && !IsParentChild();
    }

    public bool IsSubcategoryEmpty()
    {
        return SubCategories.Count != 0;
    }
}