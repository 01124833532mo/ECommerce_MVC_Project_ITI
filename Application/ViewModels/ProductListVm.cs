namespace EcommerceIti.Application.ViewModels;

public class ProductListVm
{
    public IReadOnlyCollection<ProductListItemVm> Items { get; set; } = Array.Empty<ProductListItemVm>();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int? CategoryId { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
}
