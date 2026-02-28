using EcommerceIti.Application.ViewModels;

namespace EcommerceIti.Application.Interfaces;

public interface IProductService
{
    Task<ProductListVm> GetPagedAsync(ProductListQuery query);
    Task<ProductDetailsVm?> GetDetailsAsync(int id);
    Task<ProductEditVm?> GetForEditAsync(int id);
    Task<int> CreateAsync(ProductEditVm vm);
    Task UpdateAsync(ProductEditVm vm);
    Task DeleteAsync(int id);
}
