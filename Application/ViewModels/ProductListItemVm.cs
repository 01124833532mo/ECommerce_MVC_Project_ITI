namespace EcommerceIti.Application.ViewModels;

public record ProductListItemVm(int Id, string Name, string SKU, decimal Price, int StockQuantity, bool IsActive, string CategoryName);
