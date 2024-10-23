namespace Ecommerce.Application.DTOs;

public class AddUpdateCategoryDto
{
    public string Name { get; set; } = null!;
    public string ParentName { get; set; } = "Root";

    public string TypeString { get; set; } = null!;

    // public bool Type { get; set; }
    
}