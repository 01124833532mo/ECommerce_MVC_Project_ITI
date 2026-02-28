namespace EcommerceIti.Application.ViewModels;

public class CategoryVm
{
    public int? CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public string? ParentName { get; set; }
}
