using EcommerceIti.Application.ViewModels;
using EcommerceIti.Application.Models;

namespace EcommerceIti.Application.Interfaces;

public interface IProductService
{
    Task<ProductListVm> GetPagedAsync(ProductListQuery query);
    Task<ProductDetailsVm?> GetDetailsAsync(int id);
    Task<ProductEditVm?> GetForEditAsync(int id);
    Task<bool> IsSkuInUseAsync(string sku, int? productId = null);
    Task<int> CreateAsync(ProductEditVm vm);
    Task UpdateAsync(ProductEditVm vm);
    Task<ProductDeleteResult> DeleteAsync(int id);
}
