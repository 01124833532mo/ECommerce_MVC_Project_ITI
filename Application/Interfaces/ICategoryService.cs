using EcommerceIti.Application.ViewModels;

namespace EcommerceIti.Application.Interfaces;

public interface ICategoryService
{
    Task<IList<CategoryVm>> GetAllAsync();
    Task<CategoryVm?> GetByIdAsync(int id);
    Task<int> CreateAsync(CategoryVm vm);
    Task UpdateAsync(CategoryVm vm);
    Task DeleteAsync(int id);
}
