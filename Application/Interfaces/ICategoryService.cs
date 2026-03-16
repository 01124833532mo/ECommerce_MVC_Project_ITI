using EcommerceIti.Application.ViewModels;
using EcommerceIti.Application.Models;

namespace EcommerceIti.Application.Interfaces;

public interface ICategoryService
{
    Task<IList<CategoryVm>> GetAllAsync();
    Task<CategoryVm?> GetByIdAsync(int id);
    Task<int> CreateAsync(CategoryVm vm);
    Task UpdateAsync(CategoryVm vm);
    Task<CategoryDeleteResult> DeleteAsync(int id);
}
